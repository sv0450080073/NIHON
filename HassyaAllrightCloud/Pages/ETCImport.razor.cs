using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.Pages.Components.InputFileComponent;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UtfUnknown;

namespace HassyaAllrightCloud.Pages
{
    public class ETCImportBase : ComponentBase
    {
        protected Model Model = new Model();
        [Inject] IJSRuntime _jSRuntime { get; set; }
        [Inject] protected IWebHostEnvironment _webHostEnvironment { get; set; }
        [Inject] protected IStringLocalizer<ETCImport> _lang { get; set; }
        [Inject] protected IETCImportConditionSettingService _service { get; set; }
        [Inject] private IReportLoadingService _reportLoadingService { set; get; }
        [Inject] private ETCDataTranferService eTCDataTranferService { get; set; }
        [Inject] private NavigationManager navigationManager { get; set; }
        [Inject] private IErrorHandlerService _errService { get; set; }
        [Inject] private IFilterCondition _filterService { get; set; }
        [Inject] private ILoadingService _loading { get; set; }
        protected List<FileItem> FileNames { get; set; } = new List<FileItem>();
        protected Regex IsDecimal = new Regex(@"^[0-9]+$");
        protected Regex IsHHMMSS = new Regex(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$");
        protected Regex Is5Num = new Regex(@"^[0-9]{5}$");
        protected readonly List<CommonModel> jigyosIriConst = new List<CommonModel>()
        {
            new CommonModel(){Value = 1, Text = "東日本高速道路株式会社" },
            new CommonModel(){Value = 1, Text = "中日本高速道路株式会社"},
            new CommonModel(){Value = 1, Text = "西日本高速道路株式会社"},
            new CommonModel(){Value = 3, Text = "本州四国連絡高速道路株式会社"}
        };
        private readonly string UpdPrgID = "KO0200P";
        private readonly string SiyoStaYmd = "19800101";
        private readonly string SiyoEndYmd = "20791231";
        private readonly List<string> dateFormat = new List<string>() { "yyyy/MM/dd", "yyyy/MM/d", "yyyy/M/d", "yyyy/M/dd" };
        protected ETCSettingModel ETCSetting { get; set; }
        public List<ContentTableModel> Contents { get; set; } = new List<ContentTableModel>();
        protected ContentTableModel SelectedContent { get; set; }
        IFileListEntry[] selectedFiles;
        protected bool isContentLoaded = false;
        protected bool IsFolderEmpty = false;
        protected bool isShowConfirm = false;
        protected bool isExecuteAll = false;
        protected bool IsSingleProcessBtnEnable = true;
        protected bool ShowInfo = false;
        protected bool IsBtnsEnable = false;
        protected bool isInsertError = false;
        protected bool isCsvError = false;
        protected bool loadDataErr = false;
        private int insertCount = 0;
        private string minImportedDate = string.Empty;
        private string maxImportedDate = string.Empty;
        private List<string> importedFiles = new List<string>();
        public Guid _key { get; set; } = Guid.NewGuid();
        protected string folderName { get; set; } = string.Empty;
        protected string infoMsg { get; set; } = string.Empty;
        protected string confirmMsg { get; set; } = string.Empty;
        protected InputFile inputFile;
        protected override async Task OnInitializedAsync()
        {
            try
            {
                ETCSetting = _service.GetETCSetting(_webHostEnvironment);
                Model = await BuildSearchModel(Model);
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        private async Task SaveSearchModel(Model model)
        {
            var filers = GetSearchFormModel(model);
            await _filterService.SaveFilterCondtion(filers, FormFilterName.EtcImport, 0, new ClaimModel().SyainCdSeq);
        }
        private Dictionary<string, string> GetSearchFormModel(Model model)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            if (model == null) return result;

            result.Add(nameof(Model.ConnectEtcType), ((int)model.ConnectEtcType).ToString());

            return result;
        }

        private async Task<Model> BuildSearchModel(Model model)
        {
            var filters = await _filterService.GetFilterCondition(FormFilterName.EtcImport, 0, new ClaimModel().SyainCdSeq);

            foreach (var item in filters)
            {
                var propertyInfo = model.GetType().GetProperty(item.ItemNm);
                if (propertyInfo != null && !string.IsNullOrEmpty(item.JoInput))
                {
                    switch (item.ItemNm)
                    {
                        case nameof(Model.ConnectEtcType):
                            var type = (ConnectEtcType)(int.Parse(item.JoInput));
                            propertyInfo.SetValue(model, type);
                            break;
                    }
                }
            }

            return model;
        }

        protected async Task TypeChanged(ConnectEtcType type)
        {
            try
            {
                Model.ConnectEtcType = type;
                await SaveSearchModel(Model);
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (firstRender)
                    await _jSRuntime.InvokeVoidAsync("resizeOneColumn");
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        /// <summary>
        /// Trigger when User Click Clear Btn
        /// </summary>
        protected void ClearBtnOnClick()
        {
            try
            {
                FileNames.Clear();
                Contents.Clear();
                folderName = string.Empty;
                IsBtnsEnable = false;
                IsSingleProcessBtnEnable = false;
                if(inputFile != null)
                {
                    inputFile.Reset();
                }
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        /// <summary>
        /// Trigger when user Upload Folder
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        protected async Task HandleSelection(IFileListEntry[] files)
        {
            try
            {
                await _loading.ShowAsync();
                if (!files.Any()) { IsFolderEmpty = true; ClearBtnOnClick(); return; }
                selectedFiles = files.Where(f => f.Name.Split(".")[1] == "csv").ToArray();
                if (!selectedFiles.Any())
                {
                    IsFolderEmpty = true;
                    ClearBtnOnClick();
                    return;
                }
                IsFolderEmpty = false;
                folderName = selectedFiles.FirstOrDefault().RelativePath.Split("/")[0];
                FileNames = FileListEntry2FileItem(selectedFiles);
                Model.SelectedFile = FileNames.FirstOrDefault().Id;
                await UpdateContentTable(Model.SelectedFile);
                if (FileNames.Any()) IsBtnsEnable = true;
                await _loading.HideAsync();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        /// <summary>
        /// Update content table when user change selected file
        /// </summary>
        /// <param name="selectedFile"></param>
        /// <returns></returns>
        private async Task UpdateContentTable(Guid selectedFile)
        {
            var fileItem = await GetFile(selectedFile);
            if (fileItem == null) return;
            Contents = new List<ContentTableModel>();
            await InvokeAsync(StateHasChanged);
            await Task.Delay(200);
            Contents = fileItem.Content;
            if (Contents.Any())
            {
                SelectedContent = Contents.FirstOrDefault();
                IsSingleProcessBtnEnable = Contents.Any(e => string.IsNullOrEmpty(e.Error));
                isContentLoaded = true;
            }
            else
            {
                IsSingleProcessBtnEnable = false;
            }
        }

        private async Task<FileItem?> GetFile(Guid selectedFile)
        {
            var fileItem = FileNames.Find(e => e.Id == selectedFile);
            if (fileItem != null && fileItem.Content == null)
                fileItem.Content = await Stream2ContentTableModel(fileItem.File.Data);
            return fileItem;
        }

        private List<FileItem> FileListEntry2FileItem(IFileListEntry[] files)
        {
            var result = new List<FileItem>();
            if (!files.Any()) return result;
            foreach (var file in selectedFiles)
            {
                var fileItem = new FileItem();
                fileItem.Id = Guid.NewGuid();
                fileItem.Name = file.Name;
                fileItem.File = file;
                result.Add(fileItem);
            }

            return result;
        }

        /// <summary>
        /// Update content table when user change selected file name
        /// </summary>
        /// <param name="selectedFileId"></param>
        /// <returns></returns>
        private async Task<List<ContentTableModel>> Stream2ContentTableModel(Stream data)
        {
            loadDataErr = false;
            var contents = new List<ContentTableModel>();
            try
            {
                var filePath = $"{_webHostEnvironment.WebRootPath}/csv/{Guid.NewGuid()}.csv";
                using var ms = new MemoryStream();
                await data.CopyToAsync(ms);
                // The reason to save file is CharsetDetector work only when use DetectFromFile

                using (Stream f = File.Create(filePath))
                {
                    ms.WriteTo(f);
                }
                var charset = CharsetDetector.DetectFromFile(filePath);
                if (charset.Detected != null)
                {
                    using (StreamReader sr = new StreamReader(filePath, charset.Detected.Encoding))
                    {
                        while (!sr.EndOfStream)
                        {
                            var currentLine = await sr.ReadLineAsync() ?? string.Empty;

                            contents.Add(new ContentTableModel()
                            {
                                Error = ValidateCurrentLine(currentLine) ? _lang["BI_T006"] : string.Empty,
                                RawText = currentLine
                            });
                        }
                    }
                }
                else
                {
                    contents = new List<ContentTableModel>();
                }
                File.Delete(filePath);
                return contents;
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("NotReadableError"))
                    loadDataErr = true;
                else
                    _errService.HandleError(ex);

                return contents;
            }
        }

        protected async Task FileNameChanged(Guid fileId)
        {
            try
            {
                Model.SelectedFile = fileId;

                await UpdateContentTable(fileId);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        private bool ValidateCurrentLine(string currentLine)
        {
            var cols = currentLine.Split(",");
            var error = false;
            if (cols.Length <= ETCSetting.CardNoCol || !IsDecimal.IsMatch(cols[ETCSetting.CardNoCol]))
                error = true;
            if (cols.Length <= ETCSetting.UnkYmdCol || !DateTime.TryParseExact(cols[ETCSetting.UnkYmdCol], DateTimeFormat.yyyyMMddSlash, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                error = true;
            if (cols.Length <= ETCSetting.UnkTimeCol || !IsHHMMSS.IsMatch(cols[ETCSetting.UnkTimeCol]))
                error = true;
            if (cols.Length <= ETCSetting.IriRyoKinCol || !Is5Num.IsMatch(cols[ETCSetting.IriRyoKinCol]))
                error = true;
            if (cols.Length <= ETCSetting.DeRyoKinCol || !Is5Num.IsMatch(cols[ETCSetting.DeRyoKinCol]))
                error = true;
            if (cols.Length <= ETCSetting.SyaRyoCdCol || !IsDecimal.IsMatch(cols[ETCSetting.SyaRyoCdCol]))
                error = true;
            return error;
        }

        protected void Process()
        {
            try
            {
                isExecuteAll = true;
                confirmMsg = _lang["BI_T012"];
                isShowConfirm = true;
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void SingleProcess()
        {
            try
            {
                isExecuteAll = false;
                confirmMsg = _lang["BI_T007"];
                isShowConfirm = true;
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        /// <summary>
        /// Call when user confirmed to process insert into ETC Import table
        /// </summary>
        /// <returns></returns>
        protected async Task Execute()
        {
            try
            {
                isShowConfirm = false;
                insertCount = 0;
                isCsvError = false;
                isInsertError = false;
                minImportedDate = string.Empty;
                maxImportedDate = string.Empty;
                importedFiles = new List<string>();
                var removedFiles = new List<FileItem>();
                var executingFiles = new List<FileItem>();

                if (isExecuteAll)
                    executingFiles = FileNames;
                else
                {
                    var file = await GetFile(Model.SelectedFile);
                    if (file == null) return;
                    executingFiles.Add(file);
                }
                var index = 0;
                foreach (var file in executingFiles)
                {
                    index++;
                    if (_reportLoadingService.IsCanceled(_key)) break;
                    else await _reportLoadingService.UpdateProgress(index, executingFiles.Count, _key);

                    var f = await GetFile(file.Id);
                    if (f == null) continue;

                    var insertedRows = await ProcessFile(f, !isExecuteAll);

                    if (insertedRows > -1)
                    {
                        insertCount += insertedRows;
                        removedFiles.Add(f);
                    }
                    else
                    {
                        if (insertedRows == -1 || insertedRows == -3)
                            isCsvError = true;
                        else if (insertedRows == -2)
                            isInsertError = true;

                        await _reportLoadingService.UpdateProgress(executingFiles.Count, executingFiles.Count, _key);
                        break;
                    }
                }

                if (isInsertError)
                    infoMsg = _lang["BI_T010"];
                else if (isCsvError)
                    infoMsg = _lang["BI_T009"];
                else
                {
                    infoMsg = string.Format(_lang["BI_T011"], insertCount);

                    importedFiles = removedFiles.Select(e => e.Name).ToList();
                    removedFiles.ForEach(f => FileNames.Remove(f));
                    if (FileNames.Any())
                    {
                        // Pick the first one in remaining files
                        Model.SelectedFile = FileNames.FirstOrDefault().Id;
                        IsBtnsEnable = true;
                        IsSingleProcessBtnEnable = false;
                        await UpdateContentTable(Model.SelectedFile);
                    }
                    else
                    {
                        Contents = new List<ContentTableModel>();
                        IsBtnsEnable = false;
                    }
                }

                ShowInfo = true;
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        private string GetRyokinCd(string ryoKin) => ryoKin.Substring(ryoKin.Length - 3);
        private string GetRyokinTikuCd(string ryoKin) => ryoKin.Substring(0, ryoKin.Length - 3).TrimStart('0');

        private async Task InsertRyokin(string[] cols)
        {
            var iriRyoKin = cols[ETCSetting.IriRyoKinCol];

            var isIriRyokinExist = await _service.IsRyokinExist(new RyokinSearchModel()
            {
                RyokinCd = GetRyokinCd(iriRyoKin),
                RyokinTikuCd = GetRyokinTikuCd(iriRyoKin)
            });

            if (!isIriRyokinExist)
            {
                var ryokin = RawTextToRyokin(cols, true);
                await _service.InsertRyokin(ryokin);
            }

            var deRyoKin = cols[ETCSetting.DeRyoKinCol];

            var isDeRyokinExist = await _service.IsRyokinExist(new RyokinSearchModel()
            {
                RyokinCd = GetRyokinCd(deRyoKin),
                RyokinTikuCd = GetRyokinTikuCd(deRyoKin)
            });

            if (!isDeRyokinExist)
            {
                var ryokin = RawTextToRyokin(cols, false);
                await _service.InsertRyokin(ryokin);
            }
        }

        private DateTime? ToDate(string date)
        {
            foreach (var format in dateFormat)
            {
                DateTime result;
                if (DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                    return result;
            }
            return null;
        }

        private string ToTime(string time) => string.Join("", time?.Split(":").Select(e => e.PadLeft(2, '0')) ?? new List<string>());

        /// <summary>
        /// Insert Etc Import 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="index"></param>
        /// <param name="cols"></param>
        /// <returns></returns>
        private async Task<(short, short)> InsertEtcImport(string fileName, short index, string[] cols)
        {
            var unkYmd = ToDate(cols[ETCSetting.UnkYmdCol])?.ToString(DateTimeFormat.yyyyMMdd) ?? string.Empty;

            #region Caculate update min/max imported date to navigate after import
            if (string.IsNullOrEmpty(minImportedDate))
                minImportedDate = unkYmd;
            else
                minImportedDate = minImportedDate.CompareTo(unkYmd) > -1 ? unkYmd : minImportedDate;

            if (string.IsNullOrEmpty(maxImportedDate))
                maxImportedDate = unkYmd;
            else
                maxImportedDate = maxImportedDate.CompareTo(unkYmd) > -1 ? maxImportedDate : unkYmd;
            #endregion

            short insertedRow = 0;

            var isEtcImportExist = await _service.IsETCImportExist(new ETCImportSearchModel()
            {
                TenantCdSeq = new ClaimModel().TenantID,
                CardNo = cols[ETCSetting.CardNoCol],
                UnkYmd = unkYmd,
                UnkTime = ToTime(cols[ETCSetting.UnkTimeCol]),
                SyaRyoCd = int.Parse(cols[ETCSetting.SyaRyoCdCol])
            });

            var iriRyoKin = cols[ETCSetting.IriRyoKinCol];
            var deRyoKin = cols[ETCSetting.DeRyoKinCol];
            var ryokinSplitList = await _service.GetRyokinSplit(new RyokinSplitSearchModel()
            {
                IriRyoChiCd = GetRyokinTikuCd(iriRyoKin),
                IriRyoCd = GetRyokinCd(iriRyoKin),
                DeRyoChiCd = GetRyokinTikuCd(deRyoKin),
                DeRyoCd = GetRyokinCd(deRyoKin)
            });

            if (!isEtcImportExist)
            {
                if (ryokinSplitList != null && ryokinSplitList.Any())
                {
                    foreach (var ryokinSplit in ryokinSplitList)
                    {
                        var etcImport = await RawTextToTkdEtcImport(fileName, index, cols, ryokinSplit);
                        var success = await _service.InsertOrUpdateEtcImport(etcImport, true);
                        if (success) { insertedRow++; index++; }
                    }
                }
                else
                {
                    var etcImport = await RawTextToTkdEtcImport(fileName, index, cols, null);
                    var success = await _service.InsertOrUpdateEtcImport(etcImport, true);
                    if (success) { insertedRow++; index++; }
                }
            }
            else
                index = (short)(index + (ryokinSplitList != null && ryokinSplitList.Any() ? ryokinSplitList.Count : 1));

            return (index, insertedRow);
        }

        private async Task<int> ProcessFile(FileItem file, bool isSingleFile)
        {
            string[] cols;
            var insertedRows = 0;
            if (!file.Content.Any(e => string.IsNullOrEmpty(e.Error))) return -3;
            short index = 1;
            (short, short) tuple = (0, 0);
            try
            {
                foreach (var row in file.Content)
                {
                    if(!isSingleFile || string.IsNullOrEmpty(row.Error))
                    {
                        // Due to async function can't pass ref parameter => so "index" is caculated by rows returned from InsertEtcImport function. 
                        cols = row.RawText.Split(",");

                        await InsertRyokin(cols);
                        tuple = await InsertEtcImport(file.Name, index, cols);
                        index = tuple.Item1;
                        insertedRows += tuple.Item2;
                    }
                }
                return insertedRows;
            }
            catch (Exception ex)
            {
                // Todo: log error
                if (ex is InvalidFileFormatException)
                    return -1;
                else
                    return -2;
            }
        }

        protected void ClosePopupInfo()
        {
            try
            {
                ShowInfo = false;
                if (Model.ConnectEtcType == ConnectEtcType.Start && !isCsvError && !isInsertError)
                {
                    DateTime? minDate = null;
                    if (!string.IsNullOrEmpty(minImportedDate))
                        minDate = DateTime.ParseExact(minImportedDate, DateTimeFormat.yyyyMMdd, CultureInfo.InvariantCulture);

                    DateTime? maxDate = null;
                    if (!string.IsNullOrEmpty(maxImportedDate))
                        maxDate = DateTime.ParseExact(maxImportedDate, DateTimeFormat.yyyyMMdd, CultureInfo.InvariantCulture);

                    eTCDataTranferService.Model = new ETCSearchParam()
                    {
                        ETCDateFrom = minDate,
                        ETCDateTo = maxDate,
                        SelectedTensoKbn = new ETCDropDown() { Value = 2 },
                        TenantCdSeq = new ClaimModel().TenantID,
                        ListFileName = importedFiles
                    };

                    navigationManager.NavigateTo("etc");
                }
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        private async Task<TkdEtcImport> RawTextToTkdEtcImport(string fileName, short index, string[] cols, RyokinSplitModel? ryokinSplit)
        {
            try
            {
                var iriRyoKin = cols[ETCSetting.IriRyoKinCol];
                var deRyoKin = cols[ETCSetting.DeRyoKinCol];
                var jigyosIri = cols[ETCSetting.JigyosIriCol];
                var jigyosDe = cols[ETCSetting.JigyosDeCol];
                var futaiCdSeq = (jigyosIriConst.Find(e => e.Text == jigyosIri)?.Value == 1 || jigyosIriConst.Find(e => e.Text == jigyosDe)?.Value == 1) ? ETCSetting.FutEtcVal : ETCSetting.KinEtcVal;

                var iriRyoChiCd = string.Empty;
                var iriRyoCd = string.Empty;
                var deRyoChiCd = string.Empty;
                var deRyoCd = string.Empty;
                var tanka = 0;
                if (ryokinSplit != null)
                {
                    iriRyoChiCd = ryokinSplit.EntranceRyokinTikuCd;
                    iriRyoCd = ryokinSplit.EntranceRyokinCd;
                    deRyoChiCd = ryokinSplit.ExitRyokinTikuCd;
                    deRyoCd = ryokinSplit.ExitRyokinCd;
                    tanka = ryokinSplit.Fee;
                }
                else
                {
                    iriRyoChiCd = GetRyokinTikuCd(iriRyoKin);
                    iriRyoCd = GetRyokinCd(iriRyoKin);
                    deRyoChiCd = GetRyokinTikuCd(deRyoKin);
                    deRyoCd = GetRyokinCd(deRyoKin);
                    tanka = int.Parse(cols[ETCSetting.TankaCol]);
                }

                return new TkdEtcImport()
                {
                    TenantCdSeq = new ClaimModel().TenantID,
                    FileName = fileName,
                    CardNo = cols[ETCSetting.CardNoCol],
                    EtcRen = index,
                    UnkYmd = ToDate(cols[ETCSetting.UnkYmdCol])?.ToString(DateTimeFormat.yyyyMMdd) ?? string.Empty,
                    SyaRyoCd = int.Parse(cols[ETCSetting.SyaRyoCdCol]),
                    UkeNo = 0.ToString(),
                    UnkRen = 0,
                    TeiDanNo = 0,
                    BunkRen = 0,
                    UnkTime = ToTime(cols[ETCSetting.UnkTimeCol]),
                    HenKai = 0,
                    FutTumCdSeq = futaiCdSeq,
                    FutTumNm = await _service.GetFutaiNmByFutaiCdSeq(futaiCdSeq),
                    IriRyoChiCd = byte.Parse(iriRyoChiCd),
                    IriRyoCd = iriRyoCd,
                    DeRyoChiCd = byte.Parse(deRyoChiCd),
                    DeRyoCd = deRyoCd,
                    SeisanCdSeq = 0,
                    SeisanNm = string.Empty,
                    SeisanKbn = 0,
                    Suryo = 1,
                    TanKa = tanka,
                    TesuRitu = 0,
                    SyaRyoTes = 0,
                    TensoKbn = 0,
                    ImportTanka = int.Parse(cols[ETCSetting.TankaCol]),
                    BikoNm = string.Empty,
                    ExpItem = string.Empty,
                    SiyoKbn = 1,
                    UpdYmd = CommonUtil.CurrentYYYYMMDD,
                    UpdTime = CommonUtil.CurrentHHMMSS,
                    UpdSyainCd = new ClaimModel().SyainCdSeq,
                    UpdPrgId = UpdPrgID
                };
            }
            catch (Exception e)
            {
                throw new InvalidFileFormatException(nameof(RawTextToTkdEtcImport), string.Join(",", cols), e);
            }
        }

        private VpmRyokin RawTextToRyokin(string[] cols, bool isIriRyoKin)
        {
            try
            {
                if (isIriRyoKin)
                {
                    var iriRyoKin = cols[ETCSetting.IriRyoKinCol];
                    var jigyosIri = cols[ETCSetting.JigyosIriCol];
                    var temp = jigyosIriConst.Find(e => e.Text == jigyosIri)?.Value ?? 0;
                    return new VpmRyokin()
                    {
                        RyokinTikuCd = byte.Parse(GetRyokinTikuCd(iriRyoKin)),
                        RyokinCd = GetRyokinCd(iriRyoKin),
                        RoadCorporationKbn = byte.Parse(temp.ToString()),
                        RoadCorporationName = jigyosIri,
                        DouroName = cols[13], // Todo
                        RyokinNm = cols[14],
                        RyakuNm = cols[14],
                        SiyoStaYmd = SiyoStaYmd,
                        SiyoEndYmd = SiyoEndYmd,
                        UpdYmd = CommonUtil.CurrentYYYYMMDD,
                        UpdTime = CommonUtil.CurrentHHMMSS,
                        UpdSyainCd = int.Parse(new ClaimModel().SyainCd),
                        UpdPrgId = UpdPrgID
                    };
                }
                else
                {
                    var deRyoKin = cols[ETCSetting.DeRyoKinCol];
                    var jigyosDe = cols[ETCSetting.JigyosDeCol];
                    var temp = jigyosIriConst.Find(e => e.Text == jigyosDe)?.Value ?? 0;
                    return new VpmRyokin()
                    {
                        RyokinTikuCd = byte.Parse(GetRyokinTikuCd(deRyoKin)),
                        RyokinCd = GetRyokinCd(deRyoKin),
                        RoadCorporationKbn = byte.Parse(temp.ToString()),
                        RoadCorporationName = jigyosDe,
                        DouroName = cols[16], // Todo
                        RyokinNm = cols[17],
                        RyakuNm = cols[17],
                        SiyoStaYmd = SiyoStaYmd,
                        SiyoEndYmd = SiyoEndYmd,
                        UpdYmd = CommonUtil.CurrentYYYYMMDD,
                        UpdTime = CommonUtil.CurrentHHMMSS,
                        UpdSyainCd = int.Parse(new ClaimModel().SyainCd),
                        UpdPrgId = UpdPrgID
                    };
                }

            }
            catch (Exception e)
            {
                throw new InvalidFileFormatException(nameof(RawTextToRyokin), string.Join(",", cols), e);
            }
        }

        protected void OnEmpty()
        {
            try
            {
                IsFolderEmpty = true;
                ClearBtnOnClick();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }
    }

    class InvalidFileFormatException : Exception
    {
        public InvalidFileFormatException()
        {

        }

        public InvalidFileFormatException(string functionName, string data, Exception e)
            : base(String.Format($"Faild to execute {functionName} with cols = {data}"), e)
        {

        }

    }
}
