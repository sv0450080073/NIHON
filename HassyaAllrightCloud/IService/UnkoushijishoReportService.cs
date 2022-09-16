using DevExpress.Xpo;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Constants;
using System.IO;
using Microsoft.JSInterop;
using HassyaAllrightCloud.Application.OperatingInstructionReport.Queries;
using MediatR;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Commons.Helpers;
using DevExpress.DataAccess.ObjectBinding;
using HassyaAllrightCloud.Reports.DataSource;
using DevExpress.Web;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using HassyaAllrightCloud.IService.CommonComponents;

namespace HassyaAllrightCloud.IService
{
    public interface IUnkoushijishoReportService: IReportService
    {
        Task<List<OperatingInstructionReportPDF>> GetPDFData(OperatingInstructionReportData searchParams);
        Task<List<JourneysDataReport>> GetJourneysDataByUkeno(string Ukeno, short UnkRen, string SyuKoYmd, string KikYmd, short TeiDanNo,short BunkRen, string Dateparam);
        Task<List<FuttumData>> GetMFutTuDataByListBooking(string Ukeno, short UnkRen, short TeiDanNo, short BunkRen, byte Mode);
        Task<int> GetInfoMainReport(OperatingInstructionReportData operatingInstructionReportData);
        void ApplyFilter(ref OperatingInstructionReportData operatingInstructionReportData, List<ReservationClassComponentData> reservationlst,
        List<DepartureOfficeData> departureofficelst, List<OutputOrderData> outputorderlst, Dictionary<string, string> filterValues);
        Dictionary<string, string> GetFieldValues(OperatingInstructionReportData operatingInstructionReportData);
        void ExportReportAsPdf(string ukecd, int TeiDanNo, int UnkRen, int BunkRen, IJSRuntime JSRuntime);
        void ExportReportdriAsPdf(string ukecd, int TeiDanNo, int UnkRen, int BunkRen, IJSRuntime JSRuntime);
        void ExportReportDateAsPdf(string ukecd, int TeiDanNo, int UnkRen, int BunkRen, string Date, IJSRuntime JSRuntime);
        void ExportReportdriDateAsPdf(string ukecd, int TeiDanNo, int UnkRen, int BunkRen, string Date, IJSRuntime JSRuntime);
        void ExportReportdriUkelistAsPdf(int Mode, string UkenoList, int FormOutput, IJSRuntime JSRuntime);
    }
    public class UnkoushijishoReportService : IUnkoushijishoReportService
    {
        private readonly IMediator _mediator;
        private readonly KobodbContext _dbContext;
        private readonly IReservationClassComponentService _yoyakuservice;

        public UnkoushijishoReportService(IMediator mediator,KobodbContext context, IReservationClassComponentService yoyakuservice)
        {
             _mediator = mediator;
            _dbContext = context;
            _yoyakuservice = yoyakuservice;
        }
        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            var searchParams = EncryptHelper.DecryptFromUrl<OperatingInstructionReportData>(queryParams);
            if(searchParams.YoyakuFrom == null)
            {
                searchParams.YoyakuFrom = new ReservationClassComponentData();
            }
            if (searchParams.YoyakuTo == null)
            {
                searchParams.YoyakuTo = new ReservationClassComponentData();
            }
            XtraReport report = new Reports.ReportTemplate.UnkoushijishoReport.UnkoushijishoReportNew();
            if(searchParams.CrewRecordBook==true&&searchParams.OperationInstructions==true)
            {
                report = new Reports.ReportTemplate.UnkoushijishoReport.UnkoushijishoBaseReportNew();
            }
            ObjectDataSource dataSource = new ObjectDataSource();
            var data = await GetPDFData(searchParams);
            Parameter param = new Parameter()
            {
                Name = "data",
                Type = typeof(List<OperatingInstructionReportPDF>),
                Value = data
            };
            dataSource.Name = "objectDataSource1";
            dataSource.DataSource = typeof(UnkoushijishoReportDS);
            dataSource.Constructor = new ObjectConstructorInfo(param);
            dataSource.DataMember = "_data";
            report.DataSource = dataSource;
            return report;
        }

        public async Task<List<OperatingInstructionReportPDF>> GetPDFData(OperatingInstructionReportData searchParams)
        {
            var data = new List<OperatingInstructionReportPDF>();
            var listCurrentOperatingInstruction = new List<CurrentOperatingInstruction>();
            var JourneysShowReport = new List<JourneysDataReport>();
            var IncidentalData = new List<FuttumData>();
            var LoadedGoodsData = new List<FuttumData>();
            var page = 1;
            listCurrentOperatingInstruction = await GetListOperatingInstructionForSearch(searchParams);
            listCurrentOperatingInstruction.ForEach(async e =>
            {
                JourneysShowReport = await GetJourneysDataByUkeno(e.HAISHA_UkeNo ?? "", e.HAISHA_UnkRen, e.HAISHA_SyuKoYmd ?? "", e.HAISHA_KikYmd ?? "",e.HAISHA_TeiDanNo,e.HAISHA_BunkRen,"" );
                IncidentalData = await GetMFutTuDataByListBooking(e.HAISHA_UkeNo, e.HAISHA_UnkRen, e.HAISHA_TeiDanNo, e.HAISHA_BunkRen, 1);
                LoadedGoodsData = await GetMFutTuDataByListBooking(e.HAISHA_UkeNo, e.HAISHA_UnkRen, e.HAISHA_TeiDanNo, e.HAISHA_BunkRen, 2);
                OnSetDataPerPage(data, JourneysShowReport, IncidentalData, LoadedGoodsData , e, ref page, searchParams);

            });
            data.ForEach(e =>
            {
                e.TotalPage = page - 1;
            });
            return data;
        }
        private void OnSetDataPerPage(
                List<OperatingInstructionReportPDF> listData,
                List<JourneysDataReport> JourneysShowReport,
                List<FuttumData> IncidentalData,
                List<FuttumData> LoadedGoodsData
            , dynamic item
            , ref int page, OperatingInstructionReportData SearchParams)
        {
            var itemFutaiPerPage = 5;
            var itemFutTumPerPage = 5;
            var itemJourneysPerPage = 27;
            double maxPageNumber = 0;
            List<double> listPageNumber = new List<double>();
            List<JourneysDataReport> listTempJourneysShow = new List<JourneysDataReport>();
            List<FuttumData> listTempIncidental = new List<FuttumData>();
            List<FuttumData> listTempLoadedGoods = new List<FuttumData>();
            listTempJourneysShow =JourneysShowReport;
            listTempIncidental = IncidentalData;
            listTempLoadedGoods = LoadedGoodsData;
            double countPageJourneysData = Math.Ceiling(listTempJourneysShow.Count * 1.0 / itemJourneysPerPage);
            double countPageIncidentalData = Math.Ceiling(listTempIncidental.Count * 1.0 / itemFutTumPerPage);
            double countPageLoadedGoodsData = Math.Ceiling(listTempLoadedGoods.Count * 1.0 / itemFutaiPerPage);
            listPageNumber.Add(countPageJourneysData);
            listPageNumber.Add(countPageIncidentalData);
            listPageNumber.Add(countPageLoadedGoodsData);
            maxPageNumber = Math.Max(listPageNumber.Max(), 1);
            for (int i = 0; i < maxPageNumber; i++)
            {
                var onePage = new OperatingInstructionReportPDF();
                if (listTempJourneysShow.Count > itemJourneysPerPage)
                {
                    var listJourneysPerPage = listTempJourneysShow.Skip(i * itemJourneysPerPage).Take(itemJourneysPerPage).ToList();
                    SetJourneyData(onePage, listJourneysPerPage, itemJourneysPerPage);
                }
                else
                {
                    SetJourneyData(onePage, listTempJourneysShow, itemJourneysPerPage);
                    listTempJourneysShow.Clear();
                }
                if (listTempIncidental.Count > itemFutTumPerPage)
                {
                    var listIncidentalPerPage = listTempIncidental.Skip(i * itemFutTumPerPage).Take(itemFutTumPerPage).ToList();
                    SetFutTaiData(onePage, listIncidentalPerPage, itemFutTumPerPage,1);
                }
                else
                {
                    SetFutTaiData(onePage, listTempIncidental, itemFutTumPerPage,1);
                    listTempIncidental.Clear();
                }
                if (listTempLoadedGoods.Count > itemFutaiPerPage)
                {
                    var listLoadedGoodsPerPage = listTempLoadedGoods.Skip(i * itemFutaiPerPage).Take(itemFutaiPerPage).ToList();
                    SetFutTaiData(onePage, listLoadedGoodsPerPage, itemFutaiPerPage, 2);
                }
                else
                {
                    SetFutTaiData(onePage, listTempLoadedGoods, itemFutaiPerPage, 2);
                    listTempLoadedGoods.Clear();
                }
                onePage.PageNumber = page;
                onePage.currentOperatingInstruction = item;
                onePage.SearchData = SearchParams;
                onePage.SearchData.TenantCdSeq = new ClaimModel().TenantID;
                listData.Add(onePage);
                page++;
            }

        }
        private void SetFutTaiData(
             OperatingInstructionReportPDF onePage
           , List<FuttumData> listPerPage
           , int itemPerPage
           , int checkOptionFutShow)
        {
            while (listPerPage.Count < itemPerPage)
            {
                listPerPage.Add(new FuttumData());
            }
            var FutTaiTemp = new FuttumShowData();
            FutTaiTemp.FUTTUM_HasYmd1 =listPerPage[0].FUTTUM_HasYmd ?? "";
            FutTaiTemp.FUTTUM_FutTumNm1 =listPerPage[0].FUTTUM_FutTumNm ?? "";
            FutTaiTemp.FUTTUM_SeisanNm1 =listPerPage[0].FUTTUM_SeisanNm ?? "";
            FutTaiTemp.MFUTTU_Suryo1 =listPerPage[0].MFUTTU_Suryo;

            FutTaiTemp.FUTTUM_HasYmd2 = listPerPage[1].FUTTUM_HasYmd ?? "";
            FutTaiTemp.FUTTUM_FutTumNm2 = listPerPage[1].FUTTUM_FutTumNm ?? "";
            FutTaiTemp.FUTTUM_SeisanNm2 = listPerPage[1].FUTTUM_SeisanNm ?? "";
            FutTaiTemp.MFUTTU_Suryo2 = listPerPage[1].MFUTTU_Suryo;

            FutTaiTemp.FUTTUM_HasYmd3 = listPerPage[2].FUTTUM_HasYmd ?? "";
            FutTaiTemp.FUTTUM_FutTumNm3 = listPerPage[2].FUTTUM_FutTumNm ?? "";
            FutTaiTemp.FUTTUM_SeisanNm3 = listPerPage[2].FUTTUM_SeisanNm ?? "";
            FutTaiTemp.MFUTTU_Suryo3 = listPerPage[2].MFUTTU_Suryo;

            FutTaiTemp.FUTTUM_HasYmd4 = listPerPage[3].FUTTUM_HasYmd ?? "";
            FutTaiTemp.FUTTUM_FutTumNm4 = listPerPage[3].FUTTUM_FutTumNm ?? "";
            FutTaiTemp.FUTTUM_SeisanNm4 = listPerPage[3].FUTTUM_SeisanNm ?? "";
            FutTaiTemp.MFUTTU_Suryo4 = listPerPage[3].MFUTTU_Suryo;

            FutTaiTemp.FUTTUM_HasYmd5 = listPerPage[4].FUTTUM_HasYmd ?? "";
            FutTaiTemp.FUTTUM_FutTumNm5 = listPerPage[4].FUTTUM_FutTumNm ?? "";
            FutTaiTemp.FUTTUM_SeisanNm5 = listPerPage[4].FUTTUM_SeisanNm ?? "";
            FutTaiTemp.MFUTTU_Suryo5 = listPerPage[4].MFUTTU_Suryo;
            if (checkOptionFutShow == 1)
            {
                onePage.IncidentalData = FutTaiTemp;
            }
            else if (checkOptionFutShow == 2)
            {
                onePage.LoadedGoodsData = FutTaiTemp;
            }

        }

            private void SetJourneyData(
            OperatingInstructionReportPDF onePage
          , List<JourneysDataReport> listPerPage
          , int itemPerPage
          )
        {
            while (listPerPage.Count < itemPerPage)
            {
                listPerPage.Add(new JourneysDataReport());

            }
            for (int i = 0; i < 27; i++)
            {
                onePage.JourneysShowReport.Add(new JourneysDataReport());
                onePage.JourneysShowReport[i].DateKotei = listPerPage[i].DateKotei;
                onePage.JourneysShowReport[i].DateShow = listPerPage[i].DateShow;
                onePage.JourneysShowReport[i].Koutei = listPerPage[i].Koutei;
                onePage.JourneysShowReport[i].TehNm = listPerPage[i].TehNm;
                onePage.JourneysShowReport[i].TehTel = listPerPage[i].TehTel;
            }
        }
        public async Task<List<FuttumData>> GetMFutTuDataByListBooking(string Ukeno, short UnkRen, short TeiDanNo, short BunkRen, byte Mode)
        {
            return await _mediator.Send(new GetListMFutTuData() { ukeNo = Ukeno, unkren = UnkRen, teiDanNo = TeiDanNo, bunkRen = BunkRen, mode =  Mode});
        }
        public async Task<List<JourneysDataReport>> GetJourneysDataByUkeno(string Ukeno, short UnkRen, string SyuKoYmd, string KikYmd, short TeiDanNo,short BunkRen, string Dateparam)
        {
            return await _mediator.Send(new GetJourneysHaiShaDataReport() { ukeno = Ukeno, unkRen = UnkRen, syuKoYmd = SyuKoYmd, kikYmd = KikYmd, teiDanNo = TeiDanNo, bunkRen=BunkRen, dateparam=Dateparam });
        }
        public async Task<List<CurrentOperatingInstruction>> GetListOperatingInstructionForSearch(OperatingInstructionReportData searchParams)
        {
            return await _mediator.Send(new GetListOperatingInstruction() { searchParams = searchParams });
        }
        public Task<int> GetInfoMainReport(OperatingInstructionReportData operatingInstructionReportData)
        {
            var tenantCdSeq = new ClaimModel().TenantID;
            int count = 0;
            DateTime date = new DateTime();
            if (operatingInstructionReportData.ReceiptNumberFrom == operatingInstructionReportData.ReceiptNumberTo && operatingInstructionReportData.DeliveryDate == date)
            {
                count = ((from TKD_Haisha in _dbContext.TkdHaisha
                          join TKD_Yyksho in _dbContext.TkdYyksho on TKD_Haisha.UkeNo equals TKD_Yyksho.UkeNo into TKD_Yyksho_join
                          from TKD_Yyksho in TKD_Yyksho_join.DefaultIfEmpty()
                          join TKD_Unkobi in _dbContext.TkdUnkobi
                                on new { TKD_Haisha.UkeNo, TKD_Haisha.UnkRen }
                            equals new { TKD_Unkobi.UkeNo, TKD_Unkobi.UnkRen } into TKD_Unkobi_join
                          from TKD_Unkobi in TKD_Unkobi_join.DefaultIfEmpty()
                          where
                              (string.IsNullOrEmpty(operatingInstructionReportData.ReceiptNumberTo) ? true : TKD_Yyksho.UkeCd == int.Parse(operatingInstructionReportData.ReceiptNumberTo))
                            && TKD_Haisha.SiyoKbn == 1 && TKD_Haisha.TeiDanNo == operatingInstructionReportData.TeiDanNo && TKD_Haisha.BunkRen == operatingInstructionReportData.BunkRen &&
                            TKD_Haisha.BunkRen == operatingInstructionReportData.BunkRen
                            && TKD_Yyksho.TenantCdSeq == tenantCdSeq && TKD_Haisha.HaiSsryCdSeq != 0 select TKD_Haisha.UkeNo).Count());
            }
            else if (operatingInstructionReportData.UkenoList != "" && operatingInstructionReportData.FormOutput != 0)
            {
                string[] ukenolst = operatingInstructionReportData.UkenoList.Split(',');
                if (operatingInstructionReportData.FormOutput == 1)
                {
                    count = (_dbContext.TkdHaisha.Where(x => x.SiyoKbn == 1 && x.HaiSsryCdSeq != 0).Select(x => new
                    {
                        Field = string.Concat(x.UkeNo, x.UnkRen.ToString("D3"))
                    }).ToList().Where(t => ukenolst.Contains(t.Field)).Count());
                }
                else
                {
                    count = (_dbContext.TkdHaisha.Where(x => x.SiyoKbn == 1 && x.HaiSsryCdSeq != 0).Select(x => new
                    {
                        Field = string.Concat(x.UkeNo, x.UnkRen.ToString("D3"), x.BunkRen.ToString("D3"), x.TeiDanNo.ToString("D3"))
                    }).ToList().Where(t => ukenolst.Contains(t.Field)).Count());
                }

            }
            else
            {
                try
                {
                    bool YoyakuFrom = true;
                    int yoyaFromSeq = 0;
                    if(!(operatingInstructionReportData.YoyakuFrom == null || (operatingInstructionReportData.YoyakuFrom != null && operatingInstructionReportData.YoyakuFrom.YoyaKbnSeq == 0)))
                    {
                        YoyakuFrom = false;
                        yoyaFromSeq = operatingInstructionReportData.YoyakuFrom.YoyaKbnSeq;
                    }
                    bool YoyakuTo = true;
                    int yoyaToSeq = 0;
                    if (!(operatingInstructionReportData.YoyakuTo == null || (operatingInstructionReportData.YoyakuTo != null && operatingInstructionReportData.YoyakuTo.YoyaKbnSeq == 0)))
                    {
                        YoyakuTo = false;
                        yoyaToSeq = operatingInstructionReportData.YoyakuTo.YoyaKbnSeq;
                    }
                    count = ((from TKD_Haisha in _dbContext.TkdHaisha
                              join TKD_Yyksho in _dbContext.TkdYyksho on TKD_Haisha.UkeNo equals TKD_Yyksho.UkeNo into TKD_Yyksho_join
                              from TKD_Yyksho in TKD_Yyksho_join.DefaultIfEmpty()
                              join TKD_Unkobi in _dbContext.TkdUnkobi
                                    on new { TKD_Haisha.UkeNo, TKD_Haisha.UnkRen }
                                equals new { TKD_Unkobi.UkeNo, TKD_Unkobi.UnkRen } into TKD_Unkobi_join
                              from TKD_Unkobi in TKD_Unkobi_join.DefaultIfEmpty()
                              where
                                 (string.IsNullOrEmpty(operatingInstructionReportData.ReceiptNumberFrom) ? true : TKD_Yyksho.UkeCd >= long.Parse(operatingInstructionReportData.ReceiptNumberFrom)) &&
                                 (string.IsNullOrEmpty(operatingInstructionReportData.ReceiptNumberTo) ? true : TKD_Yyksho.UkeCd <= long.Parse(operatingInstructionReportData.ReceiptNumberTo)) &&
                               TKD_Haisha.SyuKoYmd == operatingInstructionReportData.DeliveryDate.ToString("yyyyMMdd")
                               && TKD_Haisha.SiyoKbn == 1
                               && TKD_Yyksho.TenantCdSeq == tenantCdSeq
                               && TKD_Haisha.HaiSsryCdSeq != 0
                               && (YoyakuFrom ? true : TKD_Yyksho.YoyaKbnSeq >= yoyaFromSeq)
                               && (YoyakuTo ? true : TKD_Yyksho.YoyaKbnSeq <= yoyaToSeq)
                              // && operatingInstructionReportData.ReservationList.Select(x => x.YoyaKbnSeq).ToArray().Contains(TKD_Yyksho.YoyaKbnSeq) -- ANH-NTL Update 2021/06/11
                              select TKD_Haisha.UkeNo).Count());
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            return Task.FromResult(count);
        }

        public void ApplyFilter(ref OperatingInstructionReportData reportData, List<ReservationClassComponentData> reservationlst,
        List<DepartureOfficeData> departureofficelst, List<OutputOrderData> outputorderlst, Dictionary<string, string> filterValues)
        {
            foreach (var keyValue in filterValues)
            {
                //int.Parse(ReceiptNumberFrom),int.Parse(ReceiptNumberTo),ReservationStart.YoyaKbnSeq,ReservationEnd.YoyaKbnSeq,
                //DepartureOffice.EigyoCdSeq,TeiDanNo,UnkRen,BunkRen,OutputOrder.IdValue
                if (keyValue.Key == nameof(reportData.ReceiptNumberFrom))
                {
                    reportData.ReceiptNumberFrom = keyValue.Value;
                }
                if (keyValue.Key == nameof(reportData.ReceiptNumberTo))
                {
                    reportData.ReceiptNumberTo = keyValue.Value;
                }
                //if (keyValue.Key == nameof(reportData.ReservationList))
                //{
                //    var values = keyValue.Value.Split('-').Select(_ => int.Parse(_));
                //    reportData.ReservationList = reservationlst.Where(r => values.Contains(r.YoyaKbnSeq)).ToList();
                //}
                if (keyValue.Key == nameof(reportData.DeliveryDate))
                {
                    DateTime deliveryDate;
                    if (DateTime.TryParseExact(keyValue.Value, "yyyyMMdd", null, DateTimeStyles.None, out deliveryDate))
                    {
                        reportData.DeliveryDate = deliveryDate;
                    }
                }
                if (keyValue.Key == nameof(reportData.DepartureOffice))
                {
                    int office;
                    if (int.TryParse(keyValue.Value, out office))
                    {
                        reportData.DepartureOffice = departureofficelst.SingleOrDefault(d => d.EigyoCdSeq == office);
                    }
                }
                if (keyValue.Key == nameof(reportData.OutputOrder))
                {
                    int outputOrder;
                    if (int.TryParse(keyValue.Value, out outputOrder))
                    {
                        reportData.OutputOrder = outputorderlst.SingleOrDefault(o => o.IdValue == outputOrder);
                    }
                }
                if (keyValue.Key == nameof(reportData.OperationInstructions))
                {
                    bool operation;
                    if (bool.TryParse(keyValue.Value, out operation))
                    {
                        reportData.OperationInstructions = operation;
                    }
                }
                if (keyValue.Key == nameof(reportData.CrewRecordBook))
                {
                    bool crew;
                    if (bool.TryParse(keyValue.Value, out crew))
                    {
                        reportData.CrewRecordBook = crew;
                    }
                }

                if (keyValue.Key == nameof(reportData.OutputSetting))
                {
                    int outValue = 0;
                    if (int.TryParse(keyValue.Value, out outValue))
                    {
                        var result = (OutputInstruction)outValue;
                        reportData.OutputSetting = result;
                    }
                }
                if (keyValue.Key == nameof(reportData.YoyakuFrom))
                {
                    int bookingfrom;
                    if (int.TryParse(keyValue.Value, out bookingfrom))
                    {
                        reportData.YoyakuFrom = reservationlst.SingleOrDefault(d => d.YoyaKbnSeq == bookingfrom);
                    }
                }
                if (keyValue.Key == nameof(reportData.YoyakuTo))
                {
                    int bookingto;
                    if (int.TryParse(keyValue.Value, out bookingto))
                    {
                        reportData.YoyakuTo = reservationlst.SingleOrDefault(d => d.YoyaKbnSeq == bookingto);
                    }
                }
            }
        }

        public Dictionary<string, string> GetFieldValues(OperatingInstructionReportData reportData)
        {
            var result = new Dictionary<string, string>
            {
                [nameof(reportData.ReceiptNumberFrom)] = $"{reportData.ReceiptNumberFrom}",
                [nameof(reportData.ReceiptNumberTo)] = $"{reportData.ReceiptNumberTo}",
                // [nameof(reportData.ReservationList)] = string.Join('-', reportData.ReservationList.Select(_ => _.YoyaKbnSeq)),
                [nameof(reportData.DeliveryDate)] = reportData.DeliveryDate.ToString("yyyyMMdd"),
                [nameof(reportData.DepartureOffice)] = $"{reportData.DepartureOffice.EigyoCdSeq}",
                [nameof(reportData.OutputOrder)] = $"{reportData.OutputOrder.IdValue}",
                [nameof(reportData.OperationInstructions)] = reportData.OperationInstructions.ToString(),
                [nameof(reportData.CrewRecordBook)] = reportData.CrewRecordBook.ToString(),
                [nameof(reportData.OutputSetting)] = ((int)reportData.OutputSetting).ToString(),
                [nameof(reportData.YoyakuFrom)] = reportData.YoyakuFrom != null ? $"{reportData.YoyakuFrom.YoyaKbnSeq}" : "0",
                [nameof(reportData.YoyakuTo)] = reportData.YoyakuTo != null ? $"{reportData.YoyakuTo.YoyaKbnSeq}" : "0",
            };
            return result;
        }

        public async void ExportReportAsPdf(string UkeNo, int TeiDanNo, int UnkRen, int BunkRen, IJSRuntime JSRuntime)
        {
            OperatingInstructionReportData reportData = new OperatingInstructionReportData();
            //List<ReservationClassComponentData> reservationlst;
            //reservationlst = new List<ReservationClassComponentData>();
            //reservationlst = await _yoyakuservice.GetListReservationClass();
            //reservationlst.Insert(0, new ReservationClassComponentData());
            //reportData.ReservationList = reservationlst.ToList();
            reportData.ReceiptNumberFrom = UkeNo.Substring(5, 10);
            reportData.ReceiptNumberTo = UkeNo.Substring(5, 10);
            reportData.TeiDanNo = TeiDanNo;
            reportData.UnkRen = UnkRen;
            reportData.BunkRen = BunkRen;
            reportData.OutputOrder = new OutputOrderData { IdValue = 1 };
            List<OperatingInstructionReportPDF> data = await GetPDFData(reportData);
            await new TaskFactory().StartNew(() =>
            {
                var report = new Reports.ReportTemplate.UnkoushijishoReport.UnkoushijishoReportNew();
                report.DataSource = data;
                report.CreateDocument();
                using (MemoryStream ms = new MemoryStream())
                {

                    report.ExportToPdf(ms);
                    byte[] exportedFileBytes = ms.ToArray();
                    string myExportString = Convert.ToBase64String(exportedFileBytes);
                    JSRuntime.InvokeVoidAsync("loadPageScript", "OperatingInstructionReport", "downloadFileOperatingInstructionReport", myExportString, "pdf");
                }

            });
        }

        public async void ExportReportdriAsPdf(string UkeNo, int TeiDanNo, int UnkRen, int BunkRen, IJSRuntime JSRuntime)
        {
            OperatingInstructionReportData reportData = new OperatingInstructionReportData();
            //List<ReservationClassComponentData> reservationlst;
            //reservationlst = new List<ReservationClassComponentData>();
            //reservationlst = await _yoyakuservice.GetListReservationClass();
            //reservationlst.Insert(0, new ReservationClassComponentData());
            //reportData.ReservationList = reservationlst.ToList();
            reportData.ReceiptNumberFrom = UkeNo.Substring(5, 10);
            reportData.ReceiptNumberTo = UkeNo.Substring(5, 10);
            reportData.TeiDanNo = TeiDanNo;
            reportData.UnkRen = UnkRen;
            reportData.BunkRen = BunkRen;
            reportData.OutputOrder = new OutputOrderData { IdValue = 1 };
            var report = new Reports.ReportFactory.ReportJomukirokuboCreator("ReportJomukirokubo", "Report Jomukirokubo").GetReport().
                CreateByUrl($"{nameof(Reports.ReportFactory.ReportJomukirokubo)}?" + reportData.Uri);
            await new TaskFactory().StartNew(() =>
            {
                string fileType = "";
                report.CreateDocument();
                using (MemoryStream ms = new MemoryStream())
                {

                    fileType = "pdf";
                    report.ExportToPdf(ms);
                    byte[] exportedFileBytes = ms.ToArray();
                    string myExportString = Convert.ToBase64String(exportedFileBytes);
                    JSRuntime.InvokeVoidAsync("loadPageScript", "JomukirokuboReport", "downloadFileOperatingInstructionReport", myExportString, fileType);
                }

            });
        }

        public async void ExportReportDateAsPdf(string UkeNo, int TeiDanNo, int UnkRen, int BunkRen, string Date, IJSRuntime JSRuntime)
        {
            OperatingInstructionReportData reportData = new OperatingInstructionReportData();
            DateTime dateTimeConvert;
            dateTimeConvert = DateTime.ParseExact(Date, "yyyyMMdd", new CultureInfo("ja-JP"));
            reportData.DeliveryDate = dateTimeConvert;
            reportData.ReceiptNumberFrom = UkeNo.Substring(5, 10);
            reportData.ReceiptNumberTo = UkeNo.Substring(5, 10);
            reportData.TeiDanNo = TeiDanNo;
            reportData.UnkRen = UnkRen;
            reportData.BunkRen = BunkRen;
            reportData.OutputOrder = new OutputOrderData { IdValue = 1 };
            //List<ReservationClassComponentData> reservationlst;
            //reservationlst = new List<ReservationClassComponentData>();
            //reservationlst = await _yoyakuservice.GetListReservationClass();
            //reservationlst.Insert(0, new ReservationClassComponentData());
            //reportData.ReservationList = reservationlst.ToList();
            //var report = new Reports.ReportFactory.ReportUnkoushijishoCreator("ReportUnkoushijisho", "Report Unkoushijisho").GetReport().
            //    CreateByUrl($"{nameof(Reports.ReportFactory.ReportUnkoushijisho)}?" + reportData.Uri);
            List<OperatingInstructionReportPDF> data = await GetPDFData(reportData);
            await new TaskFactory().StartNew(() =>
            {
                var report = new Reports.ReportTemplate.UnkoushijishoReport.UnkoushijishoReportNew();
                report.DataSource = data;
                string fileType = "";
                report.CreateDocument();
                using (MemoryStream ms = new MemoryStream())
                {

                    fileType = "pdf";
                    report.ExportToPdf(ms);
                    byte[] exportedFileBytes = ms.ToArray();
                    string myExportString = Convert.ToBase64String(exportedFileBytes);
                    JSRuntime.InvokeVoidAsync("loadPageScript", "OperatingInstructionReport", "downloadFileOperatingInstructionReport", myExportString, fileType);
                }

            });
        }

        public async void ExportReportdriDateAsPdf(string UkeNo, int TeiDanNo, int UnkRen, int BunkRen, string Date, IJSRuntime JSRuntime)
        {
            OperatingInstructionReportData reportData = new OperatingInstructionReportData();
            //DateTime dateTimeConvert;
            //dateTimeConvert = DateTime.ParseExact(Date, "yyyyMMdd", new CultureInfo("ja-JP"));
            reportData.DeliveryDate = new DateTime();
            reportData.ReceiptNumberFrom = UkeNo.Substring(5, 10);
            reportData.ReceiptNumberTo = UkeNo.Substring(5, 10);
            reportData.TeiDanNo = TeiDanNo;
            reportData.UnkRen = UnkRen;
            reportData.BunkRen = BunkRen;
            reportData.OutputOrder = new OutputOrderData { IdValue = 1 };
            //List<ReservationClassComponentData> reservationlst;
            //reservationlst = new List<ReservationClassComponentData>();
            //reservationlst = await _yoyakuservice.GetListReservationClass();
            //reservationlst.Insert(0, new ReservationClassComponentData());
            //reportData.ReservationList = reservationlst.ToList();
            var report = new Reports.ReportFactory.ReportJomukirokuboCreator("ReportJomukirokubo", "Report Jomukirokubo").GetReport().
                CreateByUrl($"{nameof(Reports.ReportFactory.ReportJomukirokubo)}?" + reportData.Uri);
            await new TaskFactory().StartNew(() =>
            {
                string fileType = "";
                report.CreateDocument();
                using (MemoryStream ms = new MemoryStream())
                {

                    fileType = "pdf";
                    report.ExportToPdf(ms);
                    byte[] exportedFileBytes = ms.ToArray();
                    string myExportString = Convert.ToBase64String(exportedFileBytes);
                    JSRuntime.InvokeVoidAsync("loadPageScript", "JomukirokuboReport", "downloadFileOperatingInstructionReport", myExportString, fileType);
                }

            });
        }
        public async void ExportReportdriUkelistAsPdf(int Mode, string UkenoList, int FormOutput, IJSRuntime JSRuntime)
        {
            OperatingInstructionReportData reportData = new OperatingInstructionReportData();
            reportData.UkenoList = UkenoList;
            reportData.FormOutput = FormOutput;
            if (Mode == 1)
            {
                reportData.OperationInstructions = true;
                reportData.CrewRecordBook = false;
                reportData.OutputOrder = new OutputOrderData { IdValue = 1 };
                List<OperatingInstructionReportPDF> data = await GetPDFData(reportData);
                await new TaskFactory().StartNew(() =>
                {
                    var report = new Reports.ReportTemplate.UnkoushijishoReport.UnkoushijishoReportNew();
                    string fileType = "";
                    report.DataSource = data;
                    report.CreateDocument();
                    using (MemoryStream ms = new MemoryStream())
                    {

                        fileType = "pdf";
                        report.ExportToPdf(ms);
                        byte[] exportedFileBytes = ms.ToArray();
                        string myExportString = Convert.ToBase64String(exportedFileBytes);
                        JSRuntime.InvokeVoidAsync("loadPageScript", "OperatingInstructionReport", "downloadFileOperatingInstructionReport", myExportString, fileType);
                    }

                });
            }
            else if (Mode == 2)
            {
                reportData.OperationInstructions = false;
                reportData.CrewRecordBook = true;
                reportData.OutputOrder = new OutputOrderData { IdValue = 1 };
                var report = new Reports.ReportFactory.ReportJomukirokuboCreator("ReportJomukirokubo", "Report Jomukirokubo").GetReport().
                    CreateByUrl($"{nameof(Reports.ReportFactory.ReportJomukirokubo)}?" + reportData.Uri);
                await new TaskFactory().StartNew(() =>
                {
                    string fileType = "";
                    report.CreateDocument();
                    using (MemoryStream ms = new MemoryStream())
                    {

                        fileType = "pdf";
                        report.ExportToPdf(ms);
                        byte[] exportedFileBytes = ms.ToArray();
                        string myExportString = Convert.ToBase64String(exportedFileBytes);
                        JSRuntime.InvokeVoidAsync("loadPageScript", "JomukirokuboReport", "downloadFileOperatingInstructionReport", myExportString, fileType);
                    }

                });
            }
            else
            {
                reportData.OperationInstructions = true;
                reportData.CrewRecordBook = true;
            }
            
        }
    }
}
