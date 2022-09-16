using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using SharedLibraries.UI.Models;
using SharedLibraries.UI.Services;
using SharedLibraries.Utility.Constant;
using SharedLibraries.Utility.Exceptions;
using SharedLibraries.Utility.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SharedLibraries.UI.Components
{
    public class UploadBase : ComponentBase
    {
        [Inject] protected IStringLocalizer<Upload> _lang { get; set; }
        [Inject] protected ISharedLibrariesApi _s3Service { get; set; }
        [Inject] protected NavigationManager _navigation { get; set; }
        [Inject] protected IJSRuntime _jsRuntime { get; set; }
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        [Parameter] public string FilePath { get; set; }
        [Parameter] public Func<string, string> CustomFileName { get; set; }
        [Parameter] public int TenantCode { get; set; }
        [Parameter] public EventCallback<bool> Loading { get; set; }
        [Parameter] public int UpdSyainCd { get; set; }
        [Parameter] public Action<Exception> OnError { get; set; }
        [Parameter] public string UpdPrgId { get; set; }
        [Parameter] public EventCallback<List<S3File>> OnUploaded { get; set; }
        protected List<S3File> files { get; set; } = new List<S3File>();

        protected List<IKoboFile> selectedFiles;
        protected bool showResult { get; set; }
        protected bool isOn { get; set; } = false;
        protected bool maxLengthError { get; set; }
        protected bool maxSizeError { get; set; }
        protected bool maxFileSizeError { get; set; }
        protected bool showInfo { get; set; }
        protected int maxSize { get; set; } = 200;
        protected readonly int maxLengthPass = 30;
        protected int maxFiles { get; set; } = 5;
        protected string password { get; set; }
        protected string passError { get; set; }

        protected List<ProcessFileEnum> errorList { get; set; } = new List<ProcessFileEnum>();
        protected override void OnInitialized()
        {
            try
            {
                if (CustomFileName == null)
                    throw new Exception($"Please define {nameof(CustomFileName)} in {nameof(Upload)} component");
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }
        protected string GetKBs(long bytes)
        {
            try
            {
                return _s3Service.ToKB(bytes).ToString("N2");
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
                return string.Empty;
            }
        }


        protected void HandleSelection(IKoboFile[] files)
        {
            try
            {
                if ((selectedFiles?.Count ?? 0) + files.Length > 5) maxLengthError = true;
                else if (files.Any(f => _s3Service.ToMB(f.Size) > 50)) maxFileSizeError = true;
                else if (_s3Service.ToMB(selectedFiles?.Sum(f => f.Size) ?? 0) + _s3Service.ToMB(files.Sum(f => f.Size)) > 200) maxSizeError = true;
                else
                {
                    maxLengthError = maxSizeError = maxFileSizeError = false;
                    if (selectedFiles == null)
                        selectedFiles = files.ToList();
                    else
                    {
                        foreach (var f in files)
                        {
                            if (!selectedFiles.Any(sf => sf.Name == f.Name))
                                selectedFiles.Add(f);
                        }
                    }
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        protected void OpenUrl(string url)
        {
            try
            {
                _jsRuntime.InvokeVoidAsync("open", url, "_blank");
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        protected void Done()
        {
            try
            {
                showResult = false;
                OnClose.InvokeAsync(true);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        protected void AutoGeneratePass()
        {
            try
            {
                isOn = true;
                password = Guid.NewGuid().ToString().Substring(0, maxLengthPass);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }
        protected void OnChange()
        {
            try
            {
                isOn = !isOn;
                if (!isOn) password = string.Empty;
                else
                    passError = string.Empty;
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        protected void RemoveFile(IKoboFile file)
        {
            try
            {
                var list = selectedFiles;
                var index = list.IndexOf(file);
                if (index > -1) list.RemoveAt(index);
                else
                {
                    var f = list.Find(e => e.Name == file.Name);
                    index = list.IndexOf(f);
                }
                selectedFiles = list;

                StateHasChanged();
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        protected async Task UploadFiles()
        {
            try
            {
                if (isOn && (string.IsNullOrEmpty(password) || password.Length < 8 || password.Length > maxLengthPass))
                {
                    passError = string.IsNullOrEmpty(password) ? _lang["pass-required"] :
                                password.Length < 8 ? _lang["pass-length-error"] :
                                _lang["pass-maxlength-error"];
                    return;
                }
                else
                {
                    passError = string.Empty;
                }
                if (selectedFiles != null && selectedFiles.Count > 0)
                {
                    await Loading.InvokeAsync(true);
                    files.Clear();
                    var result = await Task.WhenAll(selectedFiles.Select(f => ProcessFiles(f)));
                    if (result.Any(r => r != ProcessFileEnum.Successed))
                    {
                        errorList.Clear();
                        errorList.AddRange(result.Where(r => r == ProcessFileEnum.DuplicateError ||
                        r == ProcessFileEnum.UnknowError).Distinct());
                        showInfo = true;
                        if (result.Any(r => r == ProcessFileEnum.Successed))
                        {
                            await OnUploaded.InvokeAsync(files);
                            showResult = true;
                        }
                    }
                    else
                    {
                        await OnUploaded.InvokeAsync(files);
                        showResult = true;
                    }
                }
            }
            catch (Exception e)
            {
                OnError.Invoke(e);
                // Todo: log error
                showInfo = true;
            }
            finally
            {
                await Loading.InvokeAsync(false);
            }

            StateHasChanged();
        }

        protected void HideInfo()
        {
            if (errorList.Count > 1)
            {
                errorList.RemoveAt(0);
            }
            else
            {
                errorList.Clear();
                showInfo = false;
            }
        }

        private class ProcessFileResult
        {
            public ProcessFileEnum ProcessStatus { get; set; }
            public List<S3File> S3Files { get; set; }
            public string Msg { get; set; }
        }

        public enum ProcessFileEnum
        {
            Successed,
            UnknowError,
            DuplicateError,
            EmptyError
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private async Task<ProcessFileEnum> ProcessFiles(IKoboFile file)
        {
            try
            {
                if (file != null && file.Size != 0)
                {
                    var fileName = CustomFileName.Invoke(file.Name);
                    using (var stream = new MemoryStream())
                    {
                        file.OnDataRead += (sender, eventArgs) => InvokeAsync(StateHasChanged);
                        var data = new FileSendData()
                        {
                            File = await file.Data,
                            FileName = string.Format(fileName),
                            FilePath = FilePath,
                            Password = password,
                            FileSize = Convert.ToInt32(file.Size),
                            TenantId = TenantCode,
                            UpdSyainCd = UpdSyainCd,
                            UpdPrgID = UpdPrgId
                        };
                        var f = await _s3Service.UploadFileAsync(data);
                        files.Add(f);
                    }
                    StateHasChanged();
                    return ProcessFileEnum.Successed;
                }
                else
                    return ProcessFileEnum.EmptyError;
            }
            catch (Exception ex)
            {
                if (ex is DuplicateException)
                    return ProcessFileEnum.DuplicateError;
                else
                    return ProcessFileEnum.UnknowError;
            }
        }

        protected void PassChanged(ChangeEventArgs e)
        {
            try
            {
                password = (string)e.Value;
                if (password.Length < 8)
                    passError = _lang["pass-length-error"];
                else
                    passError = string.Empty;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }

        protected string GetNavigationUrl(string encryptedFileId)
        {
            return _s3Service.BuildDownloadUrl(_navigation.BaseUri, encryptedFileId);
        }
    }
}
