using Microsoft.AspNetCore.Components;
using SharedLibraries.Utility.Models;
using SharedLibraries.UI.Services;
using System.IO;
using Microsoft.JSInterop;
using SharedLibraries.Utility.Helpers;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using System;
using SharedLibraries.Utility.Exceptions;
using SharedLibraries.Utility.Constant;

namespace SharedLibraries.UI.Components
{
    public class DownloadDocumentBase : ComponentBase
    {
        [Parameter] public string FileId { get; set; }
        [Parameter] public int UpdSyainCd { get; set; }
        [Parameter] public EventCallback<bool> Loading { get; set; }
        [Parameter] public Action<Exception> OnError { get; set; }
        [Inject] protected ISharedLibrariesApi _s3Service { get; set; }
        [Inject] protected IJSRuntime _jSRuntime { get; set; }
        [Inject] protected IStringLocalizer<DownloadDocument> _lang { get; set; }
        [Inject] protected NavigationManager _nav { get; set; }
        protected bool showPopup { get; set; }
        protected string password { get; set; }
        protected string passwordError { get; set; }
        protected S3File file { get; set; }
        protected bool verifyState { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                file = await _s3Service.GetSingleFileAsync(FileId);
                showPopup = file != null;
                verifyState = file != null && !string.IsNullOrEmpty(file.Password);
            }
            catch (Exception e)
            {
                if (e is EntityNotFoundException)
                    _nav.NavigateTo("/");
            }
        }

        protected void Verify()
        {
            try
            {
                verifyState = !(file != null && (string.IsNullOrEmpty(file.Password) || Encryptor.Encrypt(password) == file.Password));

                if (verifyState)
                    passwordError = _lang["password-error"];
                else passwordError = string.Empty;

                StateHasChanged();
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
        }
        protected string GetFileIcon(S3File file)
        {
            try
            {
                return _s3Service.GetFileIcon(file.FileType);
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
                return string.Empty;
            }
        }
        protected async Task Download()
        {
            try
            {
                if (!verifyState && file != null)
                {
                    await Loading.InvokeAsync(true);
                    var model = new DownloadModel()
                    {
                        FileId = file.EncryptedId,
                        Password = string.IsNullOrEmpty(file.Password) ? string.Empty : Encryptor.Encrypt(password),
                        UpdSyainCd = UpdSyainCd,
                        UpdPrgID = CommonConst.UpdPrgID
                    };

                    var result = await _s3Service.DownloadFileAsync(model);
                    if (result != null)
                    {
                        var extension = Path.GetExtension(file.Name);
                        string myExportString = result.Content;
                        await Loading.InvokeAsync(false);
                        await _jSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, extension.Replace(".", string.Empty).ToLower(), file.Name.Replace(extension, string.Empty) + "_");
                    }
                    else
                        await Loading.InvokeAsync(false);
                }
            }
            catch (Exception ex)
            {
                OnError.Invoke(ex);
            }
            finally
            {
                Loading.InvokeAsync(false);
            }
        }
    }
}
