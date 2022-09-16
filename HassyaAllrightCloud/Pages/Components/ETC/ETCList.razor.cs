using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;

namespace HassyaAllrightCloud.Pages.Components.ETC
{
    public class ETCListBase : ComponentBase
    {
        [Inject] protected IStringLocalizer<ETCList> _lang { get; set; }
        [Inject] protected IETCService etcService { get; set; }
        [Inject] protected IJSRuntime jsRuntime { get; set; }
        [Inject] protected NavigationManager navigationManager { get; set; }
        [Inject] private IErrorHandlerService _errService { get; set; }
        [Inject] private ILoadingService _loading { get; set; }

        [Parameter] public ETCSearchParam searchParam { get; set; }
        [Parameter] public byte fontSize { get; set; }
        [Parameter] public EventCallback<bool> DataNotFound { get; set; }
        [Parameter] public List<ETCFutai> listFutai { get; set; } = new List<ETCFutai>();
        [Parameter] public List<ETCSeisan> listSeisan { get; set; } = new List<ETCSeisan>();
        public Dictionary<string, string> LangDic = new Dictionary<string, string>();
        public List<ETCData> listETC { get; set; } = new List<ETCData>();
        public List<ETCData> displayList
        {
            get
            {
                return listETC.Skip(pageNum * pageSize).Take(pageSize).ToList(); 
            }
        }
        public EditContext searchForm { get; set; }
        public class PreETCData
        {
            public string FileName { get; set; }
            public string CardNo { get; set; }
            public int EtcRen { get; set; }
            public bool Torikomi { get; internal set; }
        }
        public List<PreETCData> PreETCDataList { get; set; } = new List<PreETCData>();
        public List<ETCYoyakuData> listYoyaku { get; set; } = new List<ETCYoyakuData>();
        public ETCData selectedETC { get; set; }
        protected ETCForm etcForm { get; set; }
        public bool isShowDialog { get; set; } = false;
        public bool isShowTransferDialog { get; set; } = false;
        protected bool showInsertForm { get; set; } = false;
        protected bool isFormValid { get; set; } = false;
        protected bool ShowInfo { get; set; } = false;
        protected string infoMsg { get; set; }
        protected byte pageSize { get; set; } = Common.DefaultPageSize;
        protected int pageNum { get; set; }
        protected Pagination pagination { get; set; }
        private TkmKasSetModel tkmKasSet { get; set; }
        private List<ETCKyoSet> listKyoSet = new List<ETCKyoSet>();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                LangDic = _lang.GetAllStrings().ToDictionary(l => l.Name, l => l.Value);
                searchForm = new EditContext(searchParam);

                tkmKasSet = await etcService.GetTkmKasSet(new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID);
                if (searchParam != null && searchForm.Validate())
                    await OnSearch();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void SaveCheckedList()
        {
            try
            {
                PreETCDataList = listETC.Select(e => new PreETCData() { CardNo = e.CardNo, EtcRen = e.EtcRen, FileName = e.FileName, Torikomi = e.Torikomi }).ToList();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void ReCheckEtcList()
        {
            try
            {
                foreach (var item in PreETCDataList)
                {
                    var temp = listETC.FirstOrDefault(e => e.FileName == item.FileName && e.CardNo == item.CardNo && e.EtcRen == item.EtcRen);
                    if (temp != null && temp.TensoKbn == 0) temp.Torikomi = item.Torikomi;
                }
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        public async Task OnSearch()
        {
            try
            {
                await _loading.ShowAsync();
                SaveCheckedList();
                pageNum = pagination.currentPage = 0;
                var taskListETC = etcService.GetListETC(searchParam);
                var taskListYoyaku = etcService.GetListYoyaku(searchParam);
                var taskKyoSet = etcService.GetListKyoSet();
                await Task.WhenAll(taskListETC, taskListYoyaku, taskKyoSet);
                listETC = taskListETC.Result;
                listYoyaku = taskListYoyaku.Result;
                listKyoSet = taskKyoSet.Result;

                if (listETC.Count == 0)
                {
                    await DataNotFound.InvokeAsync(true);
                    return;
                }
                else
                {
                    listETC.ForEach(e =>
                    {
                        var listYoyakuTemp = GetListYoyakuPerETC(e, listYoyaku);

                        if (searchParam.ScreenType == 1)
                            e.ZeiKbn = searchParam.SelectedFutai.ZeiHyoKbn;
                        if (e.ExpItem != null && e.ExpItem.Length > 3)
                        {
                            e.TesuKbn = int.Parse(e.ExpItem.Substring(3, 1));
                        }
                        else
                        {
                            e.TesuKbn = 0;
                        }

                        if (e.TensoKbn != 1)
                            e.Torikomi = true;

                        InitCalculate(e, listKyoSet);

                        var count = listYoyakuTemp.Count();
                        if (count == 1)
                        {
                            InitDataOneRow(e, listYoyakuTemp[0]);
                        }
                        else if (count > 1)
                        {
                            e.DantaiNm = _lang["BI_T005"];
                            if (listYoyakuTemp[0].CountFutai != 0)
                            {
                                e.Torikomi = false;
                            }
                        }
                        else
                        {
                            listYoyakuTemp = listYoyaku.Where(_ => _.SyaRyoCd == e.SyaRyoCd && _.UnkYmd == e.UnkYmd).ToList();
                            if (listYoyakuTemp.Count() == 1)
                            {
                                InitDataOneRow(e, listYoyakuTemp[0]);
                            }
                            else
                            {
                                if (listYoyakuTemp.Count() > 1)
                                {
                                    e.DantaiNm = _lang["BI_T005"];
                                }
                                else
                                {
                                    e.DantaiNm = _lang["BI_T013"];
                                }
                                e.Torikomi = false;
                            }
                        }

                        e.listYoyaku = listYoyakuTemp;
                        if (!string.IsNullOrEmpty(e.UkeNo) && long.Parse(e.UkeNo) != 0 && listYoyakuTemp.Count > 0)
                        {
                            UpdateUkeNoUnkRenSyaRyoCd(e, listYoyakuTemp[0]);
                            e.MFutCount = listYoyakuTemp[0].CountMFutu;
                            e.DantaiNm = listYoyakuTemp[0].DanTaNm;
                            e.MstTokuiNm = listYoyakuTemp[0].TokuiNm;
                            e.MstSitenNm = listYoyakuTemp[0].SitenNm;
                        }

                        e.selectedFutai = listFutai.FirstOrDefault(_ => _.FutaiCdSeq == e.FutTumCdSeq);
                        if (e.selectedFutai == null && listFutai.Any())
                        {
                            e.selectedFutai = listFutai.Find(e => e.FutaiCdSeq == tkmKasSet.FutTumCdSeq) ?? listFutai[0];
                        }
                        e.selectedSeisan = listSeisan.FirstOrDefault(_ => _.SeisanCdSeq == e.SeisanCdSeq);
                        if (e.selectedSeisan == null && listSeisan.Any())
                        {
                            e.selectedSeisan = listSeisan.Find(e => e.SeisanCdSeq == tkmKasSet.SeisanCdSeq) ?? listSeisan[0];
                        }
                    });

                    ReCheckEtcList();
                    await DataNotFound.InvokeAsync(false);
                }
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
            finally
            {
                await _loading.HideAsync();
            }
        }

        private List<ETCYoyakuData> GetListYoyakuPerETC(ETCData item, List<ETCYoyakuData> listYoyaku)
        {
            var tempSyukoYmdTime = DateTime.ParseExact(item.UnkYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture);
            var tempKikoYmdTime = DateTime.ParseExact(item.UnkYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture);
            var RangeSyukoYmdTime = tempSyukoYmdTime.AddHours(int.Parse(item.UnkTime.Substring(0, 2)))
                                                    .AddMinutes(int.Parse(item.UnkTime.Substring(2, 2)) + searchParam.AcquisitionRange);
            var RangeKikoYmdTime = tempKikoYmdTime.AddHours(int.Parse(item.UnkTime.Substring(0, 2)))
                                                  .AddMinutes(int.Parse(item.UnkTime.Substring(2, 2)) - searchParam.AcquisitionRange);

            var listYoyakuTemp = listYoyaku.Where(_ => _.SyaRyoCd == item.SyaRyoCd
                                                    && _.UnkYmd == item.UnkYmd
                                                    && DateTime.ParseExact(_.SyuKoYmd + " " + _.SyuKoTime, CommonConstants.FormatYMDHmWithoutSlash, CultureInfo.InvariantCulture) < RangeSyukoYmdTime
                                                    && DateTime.ParseExact(_.KikYmd + " " + _.KikTime, CommonConstants.FormatYMDHmWithoutSlash, CultureInfo.InvariantCulture) > RangeKikoYmdTime
                                                 ).ToList();

            return listYoyakuTemp;
        }

        private void InitCalculate(ETCData item, List<ETCKyoSet> listKyoSet)
        {
            // set data for ZeiRitu
            if (listKyoSet.Count == 1)
            {
                if (listKyoSet[0].Zei1StaYmd.CompareTo(item.UnkYmd) < 0 && listKyoSet[0].Zei1EndYmd.CompareTo(item.UnkYmd) > -1)
                {
                    item.ZeiRitu = listKyoSet[0].Zeiritsu1;
                }
                else if (listKyoSet[0].Zei2StaYmd.CompareTo(item.UnkYmd) < 0 && listKyoSet[0].Zei2EndYmd.CompareTo(item.UnkYmd) > -1)
                {
                    item.ZeiRitu = listKyoSet[0].Zeiritsu2;
                }
                else if (listKyoSet[0].Zei3StaYmd.CompareTo(item.UnkYmd) < 0 && listKyoSet[0].Zei3EndYmd.CompareTo(item.UnkYmd) > -1)
                {
                    item.ZeiRitu = listKyoSet[0].Zeiritsu3;
                }
            }

            // set data for SyaRyoSyo, ZeikomiKin, ZeiKbnNm
            OnCalculate(item);
        }

        private void OnCalculate(ETCData item)
        {
            item.UriGakKin = item.TanKa * item.Suryo;
            if (item.ZeiKbn == 1)
            {
                item.SyaRyoSyo = (int)Math.Round(item.UriGakKin * (item.ZeiRitu / 100));
                item.ZeikomiKin = item.UriGakKin + (int)item.SyaRyoSyo;
                item.ZeiKbnNm = _lang["tax_exemption"];
            }
            else if (item.ZeiKbn == 2)
            {
                item.SyaRyoSyo = (int)Math.Round(item.UriGakKin * (item.ZeiRitu / 100) / (1 + (item.ZeiRitu / 100)));
                item.ZeikomiKin = item.UriGakKin;
                item.ZeiKbnNm = _lang["tax_included"];
            }
            else
            {
                item.ZeikomiKin = item.UriGakKin;
                item.ZeiKbnNm = _lang["tax_exempt"];
                item.ZeiRitu = 0;
                item.SyaRyoSyo = 0;
            }
        }

        private void UpdateUkeNoUnkRenSyaRyoCd(ETCData item, ETCYoyakuData yoyaku)
        {
            // Detect if etc data updated or not
            if (long.Parse(item.UkeNo) == 0 && item.UnkRen == 0 && item.TeiDanNo == 0 && item.BunkRen == 0)
            {
                item.UkeNo = yoyaku.UkeNo;
                item.UnkRen = yoyaku.UnkRen;
                // item.SyaRyoCd =1 yoyaku.SyaRyoCd;
                item.TeiDanNo = yoyaku.TeiDanNo;
                item.BunkRen = yoyaku.BunkRen;
            }
        }

        private void InitDataOneRow(ETCData item, ETCYoyakuData yoyaku)
        {
            UpdateUkeNoUnkRenSyaRyoCd(item, yoyaku);
            item.MFutCount = yoyaku.CountMFutu;
            item.DantaiNm = yoyaku.DanTaNm;
            item.ZeikomiKin = item.TanKa * item.Suryo;
            if (yoyaku.CountFutai != 0)
            {
                item.Torikomi = false;
            }

            if (string.IsNullOrEmpty(item.UkeNo) || long.Parse(item.UkeNo) == 0)
            {
                if (searchParam.SelectedTesuKbn.Value == 0)
                {
                    item.MstTokuiNm = yoyaku.TokuiNm;
                    item.MstSitenNm = yoyaku.SitenNm;
                    item.TesuRitu = yoyaku.TesuRituFut;
                    item.SyaRyoTes = 0;
                    if (item.TesuRitu != 0)
                    {
                        item.SyaRyoTes = (int)(item.ZeikomiKin * (item.TesuRitu / 100));
                    }
                }
                else
                {
                    item.TesuRitu = 0;
                    item.SyaRyoTes = 0;
                }
            }
        }

        protected async Task OnToggleConfirmDialog(bool isDelete)
        {
            try
            {
                if (isDelete && selectedETC != null)
                {
                    var result = await etcService.DeleteETC(selectedETC);
                    if (result)
                    {
                        listETC.Remove(selectedETC);
                        selectedETC = null;
                    }
                }
                isShowDialog = !isShowDialog;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void OnRemoveRow()
        {
            try
            {
                if (selectedETC != null)
                {
                    isShowDialog = !isShowDialog;
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void OnCheck(ETCData item)
        {
            try
            {
                item.Torikomi = !item.Torikomi;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void OnETCDataChanged(string value, ETCData item, string propName)
        {
            try
            {
                if (item.TensoKbn == 0)
                {
                    switch (propName)
                    {
                        case "TanKa":
                            item.TanKa = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
                            OnCalculate(item);
                            break;
                        case "Suryo":
                            item.Suryo = (short)(string.IsNullOrEmpty(value) ? 0 : short.Parse(value));
                            OnCalculate(item);
                            break;
                        case "TesuRitu":
                            item.TesuRitu = string.IsNullOrEmpty(value) ? 0 : decimal.Parse(value);
                            if (searchParam.SelectedTesuKbn.Value == 0)
                            {
                                item.SyaRyoTes = 0;
                                if (item.TesuRitu != 0)
                                {
                                    item.SyaRyoTes = (int)(item.ZeikomiKin * (item.TesuRitu / 100));
                                }
                            }
                            else
                            {
                                item.TesuRitu = 0;
                                item.SyaRyoTes = 0;
                            }
                            break;
                    }
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void OnTesuKbnChanged(int value, ETCData item)
        {
            try
            {
                item.TesuKbn = value;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void OnHandleChanged(dynamic value, byte type, ETCData item)
        {
            try
            {
                if (type == 0)
                {
                    item.selectedFutai = value as ETCFutai;
                    item.ZeiKbn = item.selectedFutai.ZeiHyoKbn;
                    InitCalculate(item, listKyoSet);
                }
                else
                {
                    item.selectedSeisan = value as ETCSeisan;
                }
                
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void OnSelectRow(ETCData item)
        {
            try
            {
                selectedETC = item;
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected async Task OnSave()
        {
            try
            {
                if (listETC.Count > 0)
                {
                    await _loading.ShowAsync();
                    await etcService.UpdateListETC(listETC);
                    await OnSearch();
                    ShowInfo = true;
                    infoMsg = _lang["BI_T007"];
                }
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
            finally
            {
                await _loading.HideAsync();
            }
        }

        protected async Task OnTransfer()
        {
            try
            {
                isShowTransferDialog = false;
                var listTransfer = listETC.Where(_ => _.Torikomi).ToList();
                if (listTransfer.Count > 0)
                {
                    await _loading.ShowAsync();
                    var listFinal = new List<ETCData>();

                    foreach (var item in listTransfer)
                    {

                        if (item.FutTumCdSeq == 0 && item.selectedFutai != null)
                        {
                            item.FutTumCdSeq = item.selectedFutai.FutaiCdSeq;
                            item.FutTumNm = item.selectedFutai.FutaiNm;
                            item.FutaiCd = item.selectedFutai.FutaiCd;
                        }

                        if (item.SeisanCdSeq == 0 && item.selectedSeisan != null)
                        {
                            item.SeisanCdSeq = item.selectedSeisan.SeisanCdSeq;
                            item.SeisanCd = item.selectedSeisan.SeisanCd;
                            item.SeisanNm = item.selectedSeisan.SeisanNm;
                            item.SeisanKbn = item.selectedSeisan.SeisanKbn;
                        }

                        infoMsg = string.Empty;
                        if (item.FutTumCdSeq != 0 && item.SeisanCdSeq != 0)
                        {
                            ETCYoyakuData yoyaku = null;

                            // get processing yoyaku
                            if (item.listYoyaku.Count == 1)
                            {
                                yoyaku = item.listYoyaku[0];
                            }
                            else if (item.listYoyaku.Count > 1)
                            {
                                yoyaku = item.listYoyaku.FirstOrDefault(_ => _.UkeNo == item.UkeNo && _.UnkRen == item.UnkRen);
                            }

                            if (yoyaku != null)
                            {
                                // get tomkbn, nittei
                                var currentDate = DateTime.Now.ToString(CommonConstants.FormatYMD);
                                if (yoyaku.HaiSYmd.CompareTo(currentDate) < 1 && yoyaku.TouYmd.CompareTo(currentDate) > -1)
                                {
                                    item.TomKbn = 1;
                                    item.Nittei = (short)((DateTime.Now - DateTime.ParseExact(yoyaku.HaiSYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture)).TotalDays + 1);
                                }
                                else if (yoyaku.HaiSYmd.CompareTo(currentDate) > 0)
                                {
                                    item.TomKbn = 2;
                                    if (yoyaku.UnkSyuKoYmd.CompareTo(currentDate) < 0)
                                    {
                                        item.Nittei = (short)((DateTime.ParseExact(yoyaku.HaiSYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture) - DateTime.ParseExact(yoyaku.UnkSyuKoYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture)).TotalDays + 1);
                                    }
                                    else
                                    {
                                        item.Nittei = (short)((DateTime.ParseExact(yoyaku.HaiSYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture) - DateTime.Now).TotalDays + 1);
                                    }
                                }
                                else if (yoyaku.TouYmd.CompareTo(currentDate) < 0)
                                {
                                    item.TomKbn = 3;
                                    if (yoyaku.UnkKikYmd.CompareTo(currentDate) > 0)
                                    {
                                        item.Nittei = (short)(Math.Ceiling((DateTime.ParseExact(yoyaku.UnkKikYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture) - DateTime.ParseExact(yoyaku.TouYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture)).TotalDays) + 1);
                                    }
                                    else
                                    {
                                        item.Nittei = (short)(Math.Ceiling((DateTime.Now - DateTime.ParseExact(yoyaku.TouYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture)).TotalDays) + 1);
                                    }
                                }

                                listFinal.Add(item);
                            }
                            else
                            {
                                infoMsg = _lang["BI_T011"];
                                ShowInfo = true;
                                break;
                            }
                        }
                    }

                    if (ShowInfo == false)
                    {
                        try
                        {
                            await etcService.TransferETC(listFinal);
                            infoMsg = _lang["BI_T012"];
                            await OnSearch();
                        }
                        catch (Exception e)
                        {
                            // Todo: log error
                            infoMsg = _lang["BI_T011"];
                        }

                        ShowInfo = true;
                    }
                } else
                {
                    ShowInfo = true;
                    infoMsg = _lang["BI_T011"];
                }
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
            finally
            {
                await _loading.HideAsync();
            }
        }

        protected async Task SaveForm()
        {
            try
            {
                await etcForm.Save();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void OnDbClick(ETCData item)
        {
            try
            {
                if (item.DantaiNm == _lang["BI_T005"] || item.DantaiNm == _lang["BI_T013"]) return;
                OnSelectRow(item);
                showInsertForm = true;
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void InsertRow()
        {
            try
            {
                selectedETC = null;
                showInsertForm = true;
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void OnBack()
        {
            try
            {
                navigationManager.NavigateTo("ETCImportConditionSetting");
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected async Task FormVisibleChanged(FormVisibleChangedEvent e)
        {
            try
            {
                showInsertForm = e.IsVisible;
                if (e.IsReload)
                    await OnSearch();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected async Task OnChangeItemPerPage(byte size)
        {
            try
            {
                pageSize = size;
                pageNum = pagination.currentPage = 0;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected async Task PageChanged(int pageNum)
        {
            try
            {
                this.pageNum = pageNum;
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }
    }
}
