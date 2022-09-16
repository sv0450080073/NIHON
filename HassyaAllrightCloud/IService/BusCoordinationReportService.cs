using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Application.BusCoordinationReport.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Reports.DataSource;
using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Constants;
using Microsoft.JSInterop;
using System.IO;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Application.Unkobi.Queries;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using HassyaAllrightCloud.IService.CommonComponents;
namespace HassyaAllrightCloud.IService
{
    public interface IBusCoordinationReportService : IReportService
    {
        Task<List<BusCoordinationReportPDF>> GetPDFData(BusCoordinationSearchParam searchParams);
        Task<List<BusTypeDataReport>> GetBusTypeDataByListBooking(string bookingParams, int TenantID);
        Task<List<YouShaDataReport>> GetYouShaDataByListBooking(string bookingParams, string unkRenParams, int TenantID);
        Task<List<FutTumDataReport>> GetFutumDataByListBooking(string bookingParams, string unkRenParams, int FutTumKbn);
        Task<List<KyoSetData>> GetKyoSetDataByTenantCdSeq(int tenantID);
        Task<List<JourneysDataReport>> GetJourneysDataByUkeno(string Ukeno, string UnkRen, string SyuKoYmd, string KikYmd, string TeiDanNo);
        Task<List<YykSyuDataReport>> GetYykSyuDataByListBooking(string bookingParams, string unkRenParams, int TenantID);
        Task<List<CurrentBusCoordination>> GetListBusCoordinationForSearch(BusCoordinationSearchParam searchParams);
        Task<bool> CheckExistDataReport(BusCoordinationSearchParam searchParams);
        void ApplyFilter(ref BusCoordinationSearchParam operatingInstructionReportData, List<ReservationClassComponentData> ListReservationClass,
        List<CustomerComponentGyosyaData> ListGyosya, ICustomerComponentService _service, List<LoadServiceOffice> salebranchlst, List<LoadStaffList> stafflst, Dictionary<string, string> filterValues);
        Dictionary<string, string> GetFieldValues(BusCoordinationSearchParam operatingInstructionReportData);

        void ExportPdfFiles(string UnkobiDate, string Ukeno, string UnkRen, IJSRuntime JSRuntime);
        void ExportPdfFilesbyUkenoLst(string ukenolst,int formOutput, IJSRuntime JSRuntime);
        Task<DateTime> GetUnkobiDate(string Ukeno, string UnkRen);
        Task<DatetimeData> GetUnkobiDatebyUkenoList(string UkenoList);
    }
    public class BusCoordinationReportService : IBusCoordinationReportService
    {
        private readonly IMediator _mediator;
        private readonly IServiceProvider _provider;
        private IReportLayoutSettingService _reportLayoutSettingService;
        List<CustomerComponentTokiskData> TokiskData = new List<CustomerComponentTokiskData>();
        List<CustomerComponentTokiStData> TokiStData = new List<CustomerComponentTokiStData>();
        public BusCoordinationReportService(IMediator mediator, IServiceProvider provider, IReportLayoutSettingService reportLayoutSettingService)
        {
            _mediator = mediator;
            _provider = provider;
            _reportLayoutSettingService = reportLayoutSettingService;
        }
        public async Task<List<CurrentBusCoordination>> GetListBusCoordinationForSearch(BusCoordinationSearchParam searchParams)
        {
            return await _mediator.Send(new GetListBusCoordination() { searchParams = searchParams });
        }
        public async Task<List<BusTypeDataReport>> GetBusTypeDataByListBooking(string bookingParams, int TenantID)
        {
            return await _mediator.Send(new GetBusTypeByBooking() { bookingParams = bookingParams, tenantID = TenantID });
        }
        public async Task<List<YouShaDataReport>> GetYouShaDataByListBooking(string bookingParams, string unkRenParams, int TenantID)
        {
            return await _mediator.Send(new GetYousha() { bookingParams = bookingParams, unkRenParams = unkRenParams, tenantID = TenantID });
        }
        public async Task<List<FutTumDataReport>> GetFutumDataByListBooking(string bookingParams, string unkRenParams, int FutTumKbn)
        {
            return await _mediator.Send(new GetFutTum() { bookingParams = bookingParams, unkRenParams = unkRenParams, futTumKbn = FutTumKbn });
        }
        public async Task<List<KyoSetData>> GetKyoSetDataByTenantCdSeq(int TenantID)
        {
            return await _mediator.Send(new GetKyoSet() { tenantID = TenantID });
        }
        public async Task<List<JourneysDataReport>> GetJourneysDataByUkeno(string Ukeno, string UnkRen, string SyuKoYmd, string KikYmd, string TeiDanNo)
        {
            return await _mediator.Send(new GetJourneysDataReport() { ukeno = Ukeno, unkRen = UnkRen, syuKoYmd = SyuKoYmd, kikYmd = KikYmd, teiDanNo = TeiDanNo });
        }
        public async Task<List<YykSyuDataReport>> GetYykSyuDataByListBooking(string bookingParams, string unkRenParams, int TenantID)
        {
            return await _mediator.Send(new GetYykSyuDataReport() { bookingParams = bookingParams, unkRenParams = unkRenParams, tenantID = TenantID });
        }
        public async Task<List<BusCoordinationReportPDF>> GetPDFData(BusCoordinationSearchParam searchParams)
        {
            var data = new List<BusCoordinationReportPDF>();
            var listCurrentBusCoordination = new List<CurrentBusCoordination>();
            var listBusType = new List<BusTypeDataReport>(); 
            var listYouSha = new List<YouShaDataReport>();
            var listFutTai = new List<FutTumDataReport>();
            var listFutTum = new List<FutTumDataReport>();
            var listKyoSet = new List<KyoSetData>();
            var listJourneysData = new List<JourneysDataReport>();
            var listYykSyuData = new List<YykSyuDataReport>();
            var currentDate = DateTime.Now.ToString(CommonConstants.FormatYMDHm);
            var page = 1;
            listCurrentBusCoordination = await GetListBusCoordinationForSearch(searchParams);
            if (searchParams.UnkRen > 0)
            {
                listCurrentBusCoordination = listCurrentBusCoordination.Where(x => x.UNKOBI_UnkRen == searchParams.UnkRen.ToString()).ToList();
            }           
            string bookingParam = "";
            string unkRenParams = string.Empty;
            if (listCurrentBusCoordination.Count > 0)
            {
                bookingParam = FormatListStringBooking(listCurrentBusCoordination);
                unkRenParams = listCurrentBusCoordination
                    .Select(e => e.UNKOBI_UnkRen)
                    .Aggregate(string.Empty, (result, next) => string.IsNullOrEmpty(result) ? result = next : result += $"-{next}");
            }
            listBusType = await GetBusTypeDataByListBooking(bookingParam, new ClaimModel().TenantID);
            listYouSha = await GetYouShaDataByListBooking(bookingParam, unkRenParams, new ClaimModel().TenantID);
            listFutTai = await GetFutumDataByListBooking(bookingParam, unkRenParams, 1);
            listFutTum = await GetFutumDataByListBooking(bookingParam, unkRenParams, 2);
            listKyoSet = await GetKyoSetDataByTenantCdSeq(new ClaimModel().TenantID);
            listYykSyuData = await GetYykSyuDataByListBooking(bookingParam, unkRenParams, new ClaimModel().TenantID);
            listCurrentBusCoordination.ForEach(async e =>
             {
                 listJourneysData = await GetJourneysDataByUkeno(e.UNKOBI_Ukeno ?? "", e.UNKOBI_UnkRen, e.UNKOBI_SyuKoYmd ?? "", e.UNKOBI_KikYmd ?? "", "0");
                 OnSetDataPerPage(data, listBusType, listYouSha, listFutTai, listFutTum, listKyoSet.First(),
                     listJourneysData, listYykSyuData, e, ref page);
             });
            data.ForEach(e =>
            {
                e.TotalPage = page - 1;
            });
            return data;         
        }
        private void OnSetDataPerPage(
                List<BusCoordinationReportPDF> listData
            , List<BusTypeDataReport> listBusType
            , List<YouShaDataReport> listYouSha
            , List<FutTumDataReport> listFutai
            , List<FutTumDataReport> listFutTum
            , KyoSetData kyoData
            , List<JourneysDataReport> listJourneys
            , List<YykSyuDataReport> listYykSyu
            , dynamic item
            , ref int page)
        {
            var itemBusTypePerPage = 6;
            var itemYouShaPerPage = 6;
            var itemFutaiPerPage = 5;
            var itemFutTumPerPage = 5;
            var itemJourneysPerPage = 20;
            var itemYykSyuPerPage = 6;
            double maxPageNumber = 0;
            List<double> listPageNumber = new List<double>();
            List<BusTypeDataReport> listTempBusType = new List<BusTypeDataReport>();
            List<YouShaDataReport> listTempYouSha = new List<YouShaDataReport>();
            List<FutTumDataReport> listTempFutTai = new List<FutTumDataReport>();
            List<FutTumDataReport> listTempFutTum = new List<FutTumDataReport>();
            List<JourneysDataReport> listTempJourneys = new List<JourneysDataReport>();
            List<YykSyuDataReport> listTempYykSyu = new List<YykSyuDataReport>();
            listTempBusType = listBusType;
            listTempYouSha = listYouSha;
            listTempFutTai = listFutai;
            listTempFutTum = listFutTum;
            listTempJourneys = listJourneys;
            listTempYykSyu = listYykSyu;
            listTempBusType = listBusType.Where(_ => _.YYKSYU_Ukeno == item.UNKOBI_Ukeno).ToList();
            listTempYouSha = listYouSha.Where(_ => _.YOUSHA_UkeNo == item.UNKOBI_Ukeno).ToList();
            listTempFutTai = listFutai.Where(_ => _.FUTTUM_UkeNo == item.UNKOBI_Ukeno).OrderBy(x => x.FUTTUM_FutTumRen).ToList();
            listTempFutTum = listFutTum.Where(_ => _.FUTTUM_UkeNo == item.UNKOBI_Ukeno).OrderBy(x => x.FUTTUM_FutTumRen).ToList();
            listTempYykSyu = listYykSyu.Where(_ => _.YYKSYU_UkeNo == item.UNKOBI_Ukeno).ToList();
            double countPageBusTypeData = Math.Ceiling(listTempBusType.Count * 1.0 / itemBusTypePerPage);
            double countPageYouShaData = Math.Ceiling(listTempYouSha.Count * 1.0 / itemYouShaPerPage);
            double countPageFuTaiData = Math.Ceiling(listTempFutTai.Count * 1.0 / itemFutaiPerPage);
            double countPageFutTumData = Math.Ceiling(listTempFutTum.Count * 1.0 / itemFutTumPerPage);
            double countPageYourneysData = Math.Ceiling(listJourneys.Count * 1.0 / itemJourneysPerPage);
            double countPageYykSyuData = Math.Ceiling(listTempYykSyu.Count * 1.0 / itemYykSyuPerPage);
            listPageNumber.Add(countPageBusTypeData);
            listPageNumber.Add(countPageYouShaData);
            listPageNumber.Add(countPageFuTaiData);
            listPageNumber.Add(countPageFutTumData);
            listPageNumber.Add(countPageYourneysData);
            listPageNumber.Add(countPageYykSyuData);
            maxPageNumber = listPageNumber.Max();
            for (int i = 0; i < maxPageNumber; i++)
            {
                var onePage = new BusCoordinationReportPDF();
                if (listTempBusType.Count > itemBusTypePerPage)
                {
                    var listBusTypePerPage = listTempBusType.Skip(i * itemBusTypePerPage).Take(itemBusTypePerPage).ToList();
                    SetBusTypeData(onePage, listBusTypePerPage, itemBusTypePerPage);
                }
                else
                {
                    SetBusTypeData(onePage, listTempBusType, itemYouShaPerPage);
                    listTempBusType.Clear();
                }
                if (listTempYouSha.Count > itemYouShaPerPage)
                {
                    var listYouShaPerPage = listTempYouSha.Skip(i * itemYouShaPerPage).Take(itemYouShaPerPage).ToList();
                    SetYouShaData(onePage, listYouShaPerPage, itemBusTypePerPage);
                }
                else
                {
                    SetYouShaData(onePage, listTempYouSha, itemBusTypePerPage);
                    listTempYouSha.Clear();
                }
                if (listTempFutTai.Count > itemFutaiPerPage)
                {
                    var listFutTaiPerPage = listTempFutTai.Skip(i * itemFutaiPerPage).Take(itemFutaiPerPage).ToList();
                    SetFutTaiData(onePage, listFutTaiPerPage, itemFutaiPerPage, 1);
                }
                else
                {
                    SetFutTaiData(onePage, listTempFutTai, itemFutaiPerPage, 1);
                    listTempFutTai.Clear();
                }
                if (listTempFutTum.Count > itemFutTumPerPage)
                {
                    var listFutTumPerPage = listTempFutTum.Skip(i * itemFutTumPerPage).Take(itemFutTumPerPage).ToList();
                    SetFutTaiData(onePage, listFutTumPerPage, itemFutTumPerPage, 2);
                }
                else
                {
                    SetFutTaiData(onePage, listTempFutTum, itemFutTumPerPage, 2);
                    listTempFutTum.Clear();
                }
                SetKyoSetData(item, kyoData);
                if (listTempJourneys.Count > itemJourneysPerPage)
                {
                    var listJourneysPerPage = listTempJourneys.Skip(i * itemJourneysPerPage).Take(itemJourneysPerPage).ToList();
                    SetJourneyData(onePage, listJourneysPerPage, itemJourneysPerPage);
                }
                else
                {
                    SetJourneyData(onePage, listTempJourneys, itemJourneysPerPage);
                    listTempJourneys.Clear();
                }
                if (listTempYykSyu.Count > itemYykSyuPerPage)
                {
                    var listYykSyuPerPage = listTempYykSyu.Skip(i * itemYykSyuPerPage).Take(itemYykSyuPerPage).ToList();
                    SetYykSyuData(onePage, listYykSyuPerPage, itemYykSyuPerPage);
                }
                else
                {
                    SetYykSyuData(onePage, listTempYykSyu, itemJourneysPerPage);
                    listTempYykSyu.Clear();
                }
                onePage.PageNumber = page;
                onePage.BusCoordination = item;
                listData.Add(onePage);
                page++;
            }
        }
        private void SetBusTypeData(
              BusCoordinationReportPDF onePage
            , List<BusTypeDataReport> listPerPage
            , int itemPerPage
            )
        {
            while (listPerPage.Count < itemPerPage)
            {
                listPerPage.Add(new BusTypeDataReport());
            }
            var BusTypeTemp = new BusTypeShowReport();
            BusTypeTemp.SYASYU_SyaSyuNm01 = listPerPage[0].SYASYU_SyaSyuNm ?? "";
            BusTypeTemp.SYASYU_SyaSyuNm02 = listPerPage[1].SYASYU_SyaSyuNm ?? "";
            BusTypeTemp.SYASYU_SyaSyuNm03 = listPerPage[2].SYASYU_SyaSyuNm ?? "";
            BusTypeTemp.SYASYU_SyaSyuNm04 = listPerPage[3].SYASYU_SyaSyuNm ?? "";
            BusTypeTemp.SYASYU_SyaSyuNm05 = listPerPage[4].SYASYU_SyaSyuNm ?? "";
            BusTypeTemp.SYASYU_SyaSyuNm06 = listPerPage[5].SYASYU_SyaSyuNm ?? "";
            BusTypeTemp.YYKSYU_SyaSyuDai01 = listPerPage[0].YYKSYU_SyaSyuDai ?? "";
            BusTypeTemp.YYKSYU_SyaSyuDai02 = listPerPage[1].YYKSYU_SyaSyuDai ?? "";
            BusTypeTemp.YYKSYU_SyaSyuDai03 = listPerPage[2].YYKSYU_SyaSyuDai ?? "";
            BusTypeTemp.YYKSYU_SyaSyuDai04 = listPerPage[3].YYKSYU_SyaSyuDai ?? "";
            BusTypeTemp.YYKSYU_SyaSyuDai05 = listPerPage[4].YYKSYU_SyaSyuDai ?? "";
            BusTypeTemp.YYKSYU_SyaSyuDai06 = listPerPage[5].YYKSYU_SyaSyuDai ?? "";
            onePage.BusTypeShowReport = BusTypeTemp;
        }
        private void SetYouShaData(
             BusCoordinationReportPDF onePage
           , List<YouShaDataReport> listPerPage
           , int itemPerPage
           )
        {
            while (listPerPage.Count < itemPerPage)
            {
                listPerPage.Add(new YouShaDataReport());
            }
            var YouShaTemp = new YouShaDataShowReport();
            YouShaTemp.YOUSHA_Nm01 = listPerPage[0].YOUSHA_Nm ?? "";
            YouShaTemp.YOUSHA_Nm02 = listPerPage[1].YOUSHA_Nm ?? "";
            YouShaTemp.YOUSHA_Nm03 = listPerPage[2].YOUSHA_Nm ?? "";
            YouShaTemp.YOUSHA_Nm04 = listPerPage[3].YOUSHA_Nm ?? "";
            YouShaTemp.YOUSHA_Nm05 = listPerPage[4].YOUSHA_Nm ?? "";
            YouShaTemp.YOUSHA_Nm06 = listPerPage[5].YOUSHA_Nm ?? "";
            YouShaTemp.YOUSHA_Count01 = listPerPage[0].YOUSHA_Count ?? "";
            YouShaTemp.YOUSHA_Count02 = listPerPage[1].YOUSHA_Count ?? "";
            YouShaTemp.YOUSHA_Count03 = listPerPage[2].YOUSHA_Count ?? "";
            YouShaTemp.YOUSHA_Count04 = listPerPage[3].YOUSHA_Count ?? "";
            YouShaTemp.YOUSHA_Count05 = listPerPage[4].YOUSHA_Count ?? "";
            YouShaTemp.YOUSHA_Count06 = listPerPage[5].YOUSHA_Count ?? "";
            onePage.YouShaShowReport = YouShaTemp;
        }
        private void SetFutTaiData(
             BusCoordinationReportPDF onePage
           , List<FutTumDataReport> listPerPage
           , int itemPerPage
           , int checkOptionFutShow)
        {
            while (listPerPage.Count < itemPerPage)
            {
                listPerPage.Add(new FutTumDataReport());
            }
            var FutTaiTemp = new FutTumDataShowReport();
            FutTaiTemp.FutaiDate01 = listPerPage[0].FUTTUM_HasYmd ?? "";
            FutTaiTemp.FutaiNm01 = listPerPage[0].FUTTUM_FutTumNm ?? "";
            FutTaiTemp.FutaiSeisanNm01 = listPerPage[0].FUTTUM_SeisanNm ?? "";
            FutTaiTemp.FutSuLabel01 = "数量";
            FutTaiTemp.FutaiSu01 = listPerPage[0].FUTTUM_Suryo ?? "";
            FutTaiTemp.FutaiSetu01 = "単価";
            FutTaiTemp.FutTanka01 = listPerPage[0].FUTTUM_TanKa ?? "";
            FutTaiTemp.FutaiKin01 = listPerPage[0].FUTTUM_UriGakKin ?? "";
            FutTaiTemp.FutaiDate02 = listPerPage[1].FUTTUM_HasYmd ?? "";
            FutTaiTemp.FutaiNm02 = listPerPage[1].FUTTUM_FutTumNm ?? "";
            FutTaiTemp.FutaiSeisanNm02 = listPerPage[1].FUTTUM_SeisanNm ?? "";
            FutTaiTemp.FutSuLabel02 = "数量";
            FutTaiTemp.FutaiSu02 = listPerPage[1].FUTTUM_Suryo ?? "";
            FutTaiTemp.FutaiSetu02 = "単価";
            FutTaiTemp.FutTanka02 = listPerPage[1].FUTTUM_TanKa ?? "";
            FutTaiTemp.FutaiKin02 = listPerPage[1].FUTTUM_UriGakKin ?? "";
            FutTaiTemp.FutaiDate03 = listPerPage[2].FUTTUM_HasYmd ?? "";
            FutTaiTemp.FutaiNm03 = listPerPage[2].FUTTUM_FutTumNm ?? "";
            FutTaiTemp.FutaiSeisanNm03 = listPerPage[2].FUTTUM_SeisanNm ?? "";
            FutTaiTemp.FutSuLabel03 = "数量";
            FutTaiTemp.FutaiSu03 = listPerPage[2].FUTTUM_Suryo ?? "";
            FutTaiTemp.FutaiSetu03 = "単価";
            FutTaiTemp.FutTanka03 = listPerPage[2].FUTTUM_TanKa ?? "";
            FutTaiTemp.FutaiKin03 = listPerPage[2].FUTTUM_UriGakKin ?? "";
            FutTaiTemp.FutaiDate04 = listPerPage[3].FUTTUM_HasYmd ?? "";
            FutTaiTemp.FutaiNm04 = listPerPage[3].FUTTUM_FutTumNm ?? "";
            FutTaiTemp.FutaiSeisanNm04 = listPerPage[3].FUTTUM_SeisanNm ?? "";
            FutTaiTemp.FutSuLabel04 = "数量";
            FutTaiTemp.FutaiSu04 = listPerPage[3].FUTTUM_Suryo ?? "";
            FutTaiTemp.FutaiSetu04 = "単価";
            FutTaiTemp.FutTanka04 = listPerPage[3].FUTTUM_TanKa ?? "";
            FutTaiTemp.FutaiKin04 = listPerPage[3].FUTTUM_UriGakKin ?? "";
            FutTaiTemp.FutaiDate05 = listPerPage[4].FUTTUM_HasYmd ?? "";
            FutTaiTemp.FutaiNm05 = listPerPage[4].FUTTUM_FutTumNm ?? "";
            FutTaiTemp.FutaiSeisanNm05 = listPerPage[4].FUTTUM_SeisanNm ?? "";
            FutTaiTemp.FutSuLabel05 = "数量";
            FutTaiTemp.FutaiSu05 = listPerPage[4].FUTTUM_Suryo ?? "";
            FutTaiTemp.FutaiSetu05 = "単価";
            FutTaiTemp.FutTanka05 = listPerPage[4].FUTTUM_TanKa ?? "";
            FutTaiTemp.FutaiKin05 = listPerPage[4].FUTTUM_UriGakKin ?? "";
            if (checkOptionFutShow == 1)
            {
                onePage.FutTaiShowReport = FutTaiTemp;
            }
            else if (checkOptionFutShow == 2)
            {
                onePage.FutTumShowReport = FutTaiTemp;
            }
        }
        private void SetKyoSetData(
             CurrentBusCoordination currentBusCoordination
           , KyoSetData kyoSetData)
        {
            currentBusCoordination.SijJoKNm01 = kyoSetData.SijJoKNm01 ?? "";
            currentBusCoordination.SijJoKNm02 = kyoSetData.SijJoKNm02 ?? "";
            currentBusCoordination.SijJoKNm03 = kyoSetData.SijJoKNm03 ?? "";
            currentBusCoordination.SijJoKNm04 = kyoSetData.SijJoKNm04 ?? "";
            currentBusCoordination.SijJoKNm05 = kyoSetData.SijJoKNm05 ?? "";
        }

        private void SetJourneyData(
            BusCoordinationReportPDF onePage
          , List<JourneysDataReport> listPerPage
          , int itemPerPage
          )
        {
            while (listPerPage.Count < itemPerPage)
            {
                listPerPage.Add(new JourneysDataReport());
               
            }
            for(int  i=0;i<20;i++)
            {
                onePage.JourneysShowReport.Add(new JourneysDataReport());
                onePage.JourneysShowReport[i].DateKotei = listPerPage[i].DateKotei;
                onePage.JourneysShowReport[i].DateShow = listPerPage[i].DateShow;
                onePage.JourneysShowReport[i].Koutei = listPerPage[i].Koutei;
                onePage.JourneysShowReport[i].TehNm = listPerPage[i].TehNm;
                onePage.JourneysShowReport[i].TehTel = listPerPage[i].TehTel;
            }            
        }
        private void SetYykSyuData(
             BusCoordinationReportPDF onePage
           , List<YykSyuDataReport> listPerPage
           , int itemPerPage
           )
        {
            while (listPerPage.Count < itemPerPage)
            {
                listPerPage.Add(new YykSyuDataReport());
            }
            var YykSyuTemp = new YykSyuDataShowReport();
            YykSyuTemp.YYKSYU_SyaSyuTan01 = listPerPage[0].YYKSYU_SyaSyuTan ?? "";
            YykSyuTemp.YYKSYU_SyaSyuDai01 = listPerPage[0].YYKSYU_SyaSyuDai ?? "";
            YykSyuTemp.YYKSYU_SyaRyoUnc01 = listPerPage[0].YYKSYU_SyaRyoUnc ?? "";
            YykSyuTemp.YYKSYU_SyaSyuTan02 = listPerPage[1].YYKSYU_SyaSyuTan ?? "";
            YykSyuTemp.YYKSYU_SyaSyuDai02 = listPerPage[1].YYKSYU_SyaSyuDai ?? "";
            YykSyuTemp.YYKSYU_SyaRyoUnc02 = listPerPage[1].YYKSYU_SyaRyoUnc ?? "";
            YykSyuTemp.YYKSYU_SyaSyuTan03 = listPerPage[2].YYKSYU_SyaSyuTan ?? "";
            YykSyuTemp.YYKSYU_SyaSyuDai03 = listPerPage[2].YYKSYU_SyaSyuDai ?? "";
            YykSyuTemp.YYKSYU_SyaRyoUnc03 = listPerPage[2].YYKSYU_SyaRyoUnc ?? "";
            YykSyuTemp.YYKSYU_SyaSyuTan04 = listPerPage[3].YYKSYU_SyaSyuTan ?? "";
            YykSyuTemp.YYKSYU_SyaSyuDai04 = listPerPage[3].YYKSYU_SyaSyuDai ?? "";
            YykSyuTemp.YYKSYU_SyaRyoUnc04 = listPerPage[3].YYKSYU_SyaRyoUnc ?? "";
            YykSyuTemp.YYKSYU_SyaSyuTan05 = listPerPage[4].YYKSYU_SyaSyuTan ?? "";
            YykSyuTemp.YYKSYU_SyaSyuDai05 = listPerPage[4].YYKSYU_SyaSyuDai ?? "";
            YykSyuTemp.YYKSYU_SyaRyoUnc05 = listPerPage[4].YYKSYU_SyaRyoUnc ?? "";
            YykSyuTemp.YYKSYU_SyaSyuTan06 = listPerPage[5].YYKSYU_SyaSyuTan ?? "";
            YykSyuTemp.YYKSYU_SyaSyuDai06 = listPerPage[5].YYKSYU_SyaSyuDai ?? "";
            YykSyuTemp.YYKSYU_SyaRyoUnc06 = listPerPage[5].YYKSYU_SyaRyoUnc ?? "";
            onePage.YykSyuShowReport = YykSyuTemp;
        }
        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            var searchParams = EncryptHelper.DecryptFromUrl<BusCoordinationSearchParam>(queryParams);
            var report = await _reportLayoutSettingService.GetCurrentTemplate(ReportIdForSetting.Buscoordination, BaseNamespace.Buscoordination, new ClaimModel().TenantID, new ClaimModel().EigyoCdSeq, (byte)PaperSize.A4);

            ObjectDataSource dataSource = new ObjectDataSource();
            var data = await GetPDFData(searchParams);
            Parameter param = new Parameter()
            {
                Name = "data",
                Type = typeof(List<BusCoordinationReportPDF>),
                Value = data
            };
            dataSource.Name = "objectDataSource1";
            dataSource.DataSource = typeof(BusCoordinationReportReportDS);
            dataSource.Constructor = new ObjectConstructorInfo(param);
            dataSource.DataMember = "_data";
            report.DataSource = dataSource;
            return report;
        }
        public async Task<bool> CheckExistDataReport(BusCoordinationSearchParam searchParams)
        {
            bool checkData = false;
            var listCurrentBusCoordination = new List<CurrentBusCoordination>();
            listCurrentBusCoordination = await GetListBusCoordinationForSearch(searchParams);
            if(searchParams.UnkRen >0)
            {
                listCurrentBusCoordination = listCurrentBusCoordination.Where(x => x.UNKOBI_UnkRen == searchParams.UnkRen.ToString()).ToList();
            }               
            if (listCurrentBusCoordination != null && listCurrentBusCoordination.Count > 0)
            {
                checkData = true;
            }
            return checkData;
        }
        public string FormatListStringBooking(List<CurrentBusCoordination> currentBusCoordinations)
        {
            if (currentBusCoordinations != null)
            {
                string[] strFormatBookingArr = new string[currentBusCoordinations.Count];
                if (currentBusCoordinations.Count >= 1)
                {
                    for (int i = 0; i < currentBusCoordinations.Count; i++)
                    {
                        if(long.Parse(currentBusCoordinations[i].UNKOBI_Ukeno)>0)
                        {
                            strFormatBookingArr[i] = currentBusCoordinations[i].UNKOBI_Ukeno.ToString();
                        }                           
                    }
                    return String.Join("-", strFormatBookingArr.Distinct());
                }
            }
            return "";
        }
        public async void GetCustomerByDate(string strDate, string endDate, ICustomerComponentService _service)
        {
            if (string.IsNullOrEmpty(strDate) && string.IsNullOrEmpty(endDate))
            {
                TokiskData = await _service.GetListTokisk(strDate, endDate);
                TokiStData = await _service.GetListTokiSt(strDate, endDate);
            }
            else
            {
                TokiskData = await _service.GetListTokisk();
                TokiStData = await _service.GetListTokiSt();
            }
        }
        public void ApplyFilter(ref BusCoordinationSearchParam reportData, List<ReservationClassComponentData> ListReservationClass, List<CustomerComponentGyosyaData> ListGyosya, ICustomerComponentService _service, List<LoadServiceOffice> salebranchlst, List<LoadStaffList> stafflst, Dictionary<string, string> filterValues)
        {
            CustomerComponentGyosyaData GyosyaTokuiFrom = new CustomerComponentGyosyaData();
            CustomerComponentGyosyaData GyosyaTokuiTo = new CustomerComponentGyosyaData();
            CustomerComponentGyosyaData GyosyaShiireFrom = new CustomerComponentGyosyaData();
            CustomerComponentGyosyaData GyosyaShiireTo = new CustomerComponentGyosyaData();
            CustomerComponentTokiskData TokiskTokuiFrom = new CustomerComponentTokiskData();
            CustomerComponentTokiskData TokiskTokuiTo = new CustomerComponentTokiskData();
            CustomerComponentTokiskData TokiskShiireFrom = new CustomerComponentTokiskData();
            CustomerComponentTokiskData TokiskShiireTo = new CustomerComponentTokiskData();

            foreach (var keyValue in filterValues)
            {
                if (keyValue.Key == nameof(reportData.DateType))
                {
                    int dateType;
                    if (int.TryParse(keyValue.Value, out dateType))
                    {
                        reportData.DateType = dateType;
                    }
                }
                if (keyValue.Key == nameof(reportData.UnkRen))
                {
                    short unkRen;
                    if (short.TryParse(keyValue.Value, out unkRen))
                    {
                        reportData.UnkRen = unkRen;
                    }
                }
                if (keyValue.Key == nameof(reportData.StartDate))
                {
                    DateTime startDate;
                    if (DateTime.TryParseExact(keyValue.Value, "yyyyMMdd", null, DateTimeStyles.None, out startDate))
                    {
                        reportData.StartDate = startDate;
                    }
                }
                if (keyValue.Key == nameof(reportData.EndDate))
                {
                    DateTime endDate;
                    if (DateTime.TryParseExact(keyValue.Value, "yyyyMMdd", null, DateTimeStyles.None, out endDate))
                    {
                        reportData.EndDate = endDate;
                    }
                }
                // Get tokisk by date
                GetCustomerByDate(reportData.StartDate.ToString(CommonConstants.FormatYMD), reportData.EndDate.ToString(CommonConstants.FormatYMD), _service);
                if (keyValue.Key == nameof(reportData.YoyakuFrom))
                {
                    if (int.TryParse(keyValue.Value, out int outValue))
                    {
                        reportData.YoyakuFrom = ListReservationClass.Where(x => x.YoyaKbnSeq == outValue).FirstOrDefault();
                    }
                }
                if (keyValue.Key == nameof(reportData.YoyakuTo))
                {
                    if (int.TryParse(keyValue.Value, out int outValue))
                    {
                        reportData.YoyakuTo = ListReservationClass.Where(x => x.YoyaKbnSeq == outValue).FirstOrDefault();
                    }
                }
                // Customer. Supperlier

                if (keyValue.Key == nameof(reportData.GyosyaTokuiSakiFrom))
                {
                    if (int.TryParse(keyValue.Value, out int outValue))
                    {
                        reportData.GyosyaTokuiSakiFrom = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue);
                        GyosyaTokuiFrom = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue);
                    }
                    else
                    {
                        reportData.GyosyaTokuiSakiFrom = null;
                    }
                }
                if (keyValue.Key == nameof(reportData.GyosyaTokuiSakiTo))
                {
                    if (int.TryParse(keyValue.Value, out int outValue))
                    {
                        reportData.GyosyaTokuiSakiTo = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue);
                        GyosyaTokuiTo = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue);
                    }
                    else
                    {
                        reportData.GyosyaTokuiSakiTo = null;
                    }
                }
                if (reportData.GyosyaTokuiSakiFrom != null && keyValue.Key == nameof(reportData.TokiskTokuiSakiFrom))
                {
                    if (int.TryParse(keyValue.Value, out int outValue))
                    {
                        List<CustomerComponentTokiskData> LstTokisk = new List<CustomerComponentTokiskData>();
                        LstTokisk = TokiskData.Where(_ => _.GyosyaCdSeq == (GyosyaTokuiFrom?.GyosyaCdSeq ?? -1)).ToList();
                        reportData.TokiskTokuiSakiFrom = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == outValue);
                        TokiskTokuiFrom = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == outValue);
                    }
                    else
                    {
                        reportData.TokiskTokuiSakiFrom = null;
                    }
                }
                if (reportData.GyosyaTokuiSakiTo != null && keyValue.Key == nameof(reportData.TokiskTokuiSakiTo))
                {
                    if (int.TryParse(keyValue.Value, out int outValue))
                    {
                        List<CustomerComponentTokiskData> LstTokisk = new List<CustomerComponentTokiskData>();
                        LstTokisk = TokiskData.Where(_ => _.GyosyaCdSeq == (GyosyaTokuiTo?.GyosyaCdSeq ?? -1)).ToList();
                        reportData.TokiskTokuiSakiTo = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == outValue);
                        TokiskTokuiTo = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == outValue);
                    }
                    else
                    {
                        reportData.TokiskTokuiSakiTo = null;
                    }
                }
                if (keyValue.Key == nameof(reportData.TokiStTokuiSakiFrom) && reportData.TokiskTokuiSakiFrom != null)
                {
                    if (int.TryParse(keyValue.Value, out int outValue))
                    {
                        List<CustomerComponentTokiStData> LstTokiSt = new List<CustomerComponentTokiStData>();
                        LstTokiSt = TokiStData.Where(_ => _.TokuiSeq == (TokiskTokuiFrom?.TokuiSeq ?? -1)).ToList();
                        reportData.TokiStTokuiSakiFrom = LstTokiSt.FirstOrDefault(_ => _.SitenCdSeq == outValue);
                    }
                    else
                    {
                        reportData.TokiStTokuiSakiFrom = null;
                    }
                }
                if (keyValue.Key == nameof(reportData.TokiStTokuiSakiTo) && reportData.TokiskTokuiSakiTo != null)
                {
                    if (int.TryParse(keyValue.Value, out int outValue))
                    {
                        List<CustomerComponentTokiStData> LstTokiSt = new List<CustomerComponentTokiStData>();
                        LstTokiSt = TokiStData.Where(_ => _.TokuiSeq == (TokiskTokuiTo?.TokuiSeq ?? -1)).ToList();
                        reportData.TokiStTokuiSakiTo = LstTokiSt.FirstOrDefault(_ => _.SitenCdSeq == outValue);
                    }
                    else
                    {
                        reportData.TokiStTokuiSakiTo = null;
                    }
                }
                if (keyValue.Key == nameof(reportData.GyosyaShiireSakiFrom))
                {
                    if (int.TryParse(keyValue.Value, out int outValue))
                    {
                        reportData.GyosyaShiireSakiFrom = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue);
                        GyosyaShiireFrom = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue);
                    }
                    else
                    {
                        reportData.GyosyaShiireSakiFrom = null;
                    }
                }
                if (keyValue.Key == nameof(reportData.GyosyaShiireSakiTo))
                {
                    if (int.TryParse(keyValue.Value, out int outValue))
                    {
                        reportData.GyosyaShiireSakiTo = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue);
                        GyosyaShiireTo = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue);
                    }
                    else
                    {
                        reportData.GyosyaShiireSakiTo = null;
                    }
                }
                if (reportData.GyosyaShiireSakiFrom != null && keyValue.Key == nameof(reportData.TokiskShiireSakiFrom))
                {
                    if (int.TryParse(keyValue.Value, out int outValue))
                    {
                        List<CustomerComponentTokiskData> LstTokisk = new List<CustomerComponentTokiskData>();
                        LstTokisk = TokiskData.Where(_ => _.GyosyaCdSeq == (GyosyaShiireFrom?.GyosyaCdSeq ?? -1)).ToList();
                        reportData.TokiskShiireSakiFrom = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == outValue);
                        TokiskShiireFrom = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == outValue);
                    }
                    else
                    {
                        reportData.TokiskShiireSakiFrom = null;
                    }
                }
                if (reportData.GyosyaShiireSakiTo != null && keyValue.Key == nameof(reportData.TokiskShiireSakiTo))
                {
                    if (int.TryParse(keyValue.Value, out int outValue))
                    {
                        List<CustomerComponentTokiskData> LstTokisk = new List<CustomerComponentTokiskData>();
                        LstTokisk = TokiskData.Where(_ => _.GyosyaCdSeq == (GyosyaShiireTo?.GyosyaCdSeq ?? -1)).ToList();
                        reportData.TokiskShiireSakiTo = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == outValue);
                        TokiskShiireTo = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == outValue);
                    }
                    else
                    {
                        reportData.TokiskShiireSakiTo = null;
                    }
                }
                if (keyValue.Key == nameof(reportData.TokiStShiireSakiFrom) && reportData.TokiskShiireSakiFrom != null)
                {
                    if (int.TryParse(keyValue.Value, out int outValue))
                    {
                        List<CustomerComponentTokiStData> LstTokiSt = new List<CustomerComponentTokiStData>();
                        LstTokiSt = TokiStData.Where(_ => _.TokuiSeq == (TokiskShiireFrom?.TokuiSeq ?? -1)).ToList();
                        reportData.TokiStShiireSakiFrom = LstTokiSt.FirstOrDefault(_ => _.SitenCdSeq == outValue);
                    }
                    else
                    {
                        reportData.TokiStShiireSakiFrom = null;
                    }
                }
                if (keyValue.Key == nameof(reportData.TokiStShiireSakiTo) && reportData.TokiskShiireSakiTo != null)
                {
                    if (int.TryParse(keyValue.Value, out int outValue))
                    {
                        List<CustomerComponentTokiStData> LstTokiSt = new List<CustomerComponentTokiStData>();
                        LstTokiSt = TokiStData.Where(_ => _.TokuiSeq == (TokiskShiireTo?.TokuiSeq ?? -1)).ToList();
                        reportData.TokiStShiireSakiTo = LstTokiSt.FirstOrDefault(_ => _.SitenCdSeq == outValue);
                    }
                    else
                    {
                        reportData.TokiStShiireSakiTo = null;
                    }
                }
                if (keyValue.Key == nameof(reportData.SaleBranch))
                {
                    int office;
                    if (int.TryParse(keyValue.Value, out office))
                    {
                        reportData.SaleBranch = salebranchlst.SingleOrDefault(r => r.OfficeCdSeq == office);
                    }
                }
                if (keyValue.Key == nameof(reportData.BookingFrom))
                {
                    reportData.BookingFrom = keyValue.Value;
                }
                if (keyValue.Key == nameof(reportData.BookingTo))
                {
                    reportData.BookingTo = keyValue.Value;
                }
                if (keyValue.Key == nameof(reportData.Staff))
                {
                    int staff;
                    if (int.TryParse(keyValue.Value, out staff))
                    {
                        reportData.Staff = stafflst.SingleOrDefault(r => r.SyainCdSeq == staff);
                    }
                }
                if (keyValue.Key == nameof(reportData.PersonInput))
                {
                    int input;
                    if (int.TryParse(keyValue.Value, out input))
                    {
                        reportData.PersonInput = stafflst.SingleOrDefault(r => r.SyainCdSeq == input);
                    }
                }
                if (keyValue.Key == nameof(reportData.OutputSetting))
                {
                    int input;
                    if (int.TryParse(keyValue.Value, out input))
                    {
                        var result = (OutputInstruction)input;
                        reportData.OutputSetting = result;
                    }
                }
            }
        }
        public Dictionary<string, string> GetFieldValues(BusCoordinationSearchParam reportData)
        {
            var result = new Dictionary<string, string>
            {
                [nameof(reportData.DateType)] = $"{reportData.DateType}",
                [nameof(reportData.StartDate)] = reportData.StartDate.ToString("yyyyMMdd"),
                [nameof(reportData.EndDate)] = reportData.EndDate.ToString("yyyyMMdd"),
                [nameof(reportData.GyosyaTokuiSakiFrom)] = reportData.GyosyaTokuiSakiFrom != null ? $"{reportData.GyosyaTokuiSakiFrom.GyosyaCdSeq}" : "0",
                [nameof(reportData.GyosyaTokuiSakiTo)] = reportData.GyosyaTokuiSakiTo != null ? $"{reportData.GyosyaTokuiSakiTo.GyosyaCdSeq}" : "0",
                [nameof(reportData.TokiskTokuiSakiFrom)] = reportData.TokiskTokuiSakiFrom != null ? $"{reportData.TokiskTokuiSakiFrom.TokuiSeq}" : "0",
                [nameof(reportData.TokiskTokuiSakiTo)] = reportData.TokiskTokuiSakiTo != null ? $"{reportData.TokiskTokuiSakiTo.TokuiSeq}" : "0",
                [nameof(reportData.TokiStTokuiSakiFrom)] = reportData.TokiStTokuiSakiFrom != null ? $"{reportData.TokiStTokuiSakiFrom.SitenCdSeq}" : "0",
                [nameof(reportData.TokiStTokuiSakiTo)] = reportData.TokiStTokuiSakiTo != null ? $"{reportData.TokiStTokuiSakiTo.SitenCdSeq}" : "0",
                [nameof(reportData.GyosyaShiireSakiFrom)] = reportData.GyosyaShiireSakiFrom != null ? $"{reportData.GyosyaShiireSakiFrom.GyosyaCdSeq}" : "0",
                [nameof(reportData.GyosyaShiireSakiTo)] = reportData.GyosyaShiireSakiTo != null ? $"{reportData.GyosyaShiireSakiTo.GyosyaCdSeq}" : "0",
                [nameof(reportData.TokiskShiireSakiFrom)] = reportData.TokiskShiireSakiFrom != null ? $"{reportData.TokiskShiireSakiFrom.TokuiSeq}" : "0",
                [nameof(reportData.TokiskShiireSakiTo)] = reportData.TokiskShiireSakiTo != null ? $"{reportData.TokiskShiireSakiTo.TokuiSeq}" : "0",
                [nameof(reportData.TokiStShiireSakiFrom)] = reportData.TokiStShiireSakiFrom != null ? $"{reportData.TokiStShiireSakiFrom.SitenCdSeq}" : "0",
                [nameof(reportData.TokiStShiireSakiTo)] = reportData.TokiStShiireSakiTo != null ? $"{reportData.TokiStShiireSakiTo.SitenCdSeq}" : "0",
                [nameof(reportData.SaleBranch)] = $"{reportData.SaleBranch.OfficeCdSeq}",
                [nameof(reportData.BookingFrom)] = $"{reportData.BookingFrom}",
                [nameof(reportData.BookingTo)] = $"{reportData.BookingTo}",
                [nameof(reportData.Staff)] = $"{reportData.Staff.SyainCdSeq}",
                [nameof(reportData.PersonInput)] = $"{reportData.PersonInput.SyainCdSeq}",
                [nameof(reportData.UnkRen)] = $"{reportData.UnkRen}",
                [nameof(reportData.OutputSetting)] = $"{ ((int)reportData.OutputSetting).ToString()}",
                [nameof(reportData.YoyakuFrom)] = reportData.YoyakuFrom != null ? $"{reportData.YoyakuFrom.YoyaKbnSeq}" : "0",
                [nameof(reportData.YoyakuTo)] = reportData.YoyakuFrom != null ? $"{reportData.YoyakuTo.YoyaKbnSeq}" : "0",

            };
            return result;
        }
        public async void ExportPdfFilesbyUkenoLst(string ukenolst,int formOutput, IJSRuntime JSRuntime)
        {
            ITPM_YoyKbnDataListService TPM_YoyKbnDataService = (ITPM_YoyKbnDataListService)_provider.GetService(typeof(ITPM_YoyKbnDataListService));
            var reservationlst = await TPM_YoyKbnDataService.GetYoyKbnbySiyoKbn();
            BusCoordinationSearchParam reportParams = new BusCoordinationSearchParam();
            DatetimeData datetimeData = new DatetimeData();
            datetimeData = await GetUnkobiDatebyUkenoList(ukenolst);
            reportParams.StartDate = datetimeData.DateStart;
            reportParams.EndDate = datetimeData.DateEnd;
            reportParams.BookingFrom = "0";
            reportParams.BookingTo = int.MaxValue.ToString();
            reportParams.ReservationList = reservationlst.Select(t=>t.YoyaKbnSeq).ToList();
            reportParams.DateType = 1;
            reportParams.OutputSetting = OutputInstruction.Pdf;
            // ANH-NTL ADD STR 2021/05/07
            reportParams.UkenoList = ukenolst;
            // ANH-NTL ADD END 2021/05/07
            List<BusCoordinationReportPDF> data = await GetPDFData(reportParams);
            if (data.Count > 0)
            {
                var report = await _reportLayoutSettingService.GetCurrentTemplate(ReportIdForSetting.Buscoordination, BaseNamespace.Buscoordination, new ClaimModel().TenantID, new ClaimModel().EigyoCdSeq, (byte)PaperSize.A4);
                report.DataSource = data;
                await new System.Threading.Tasks.TaskFactory().StartNew(() =>
                {
                    report.CreateDocument();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        /*if (type == 1)
                        {
                            PrintToolBase tool = new PrintToolBase(report.PrintingSystem);
                            tool.Print();
                            return;
                        }*/
                        report.ExportToPdf(ms);
                        byte[] exportedFileBytes = ms.ToArray();
                        string myExportString = Convert.ToBase64String(exportedFileBytes);
                        JSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "pdf", "Busutehaisyo");
                    }
                });
            }
        }
        public async void ExportPdfFiles(string UnkobiDate, string Ukeno, string UnkRen, IJSRuntime JSRuntime)
        {
            ITPM_YoyKbnDataListService TPM_YoyKbnDataService = (ITPM_YoyKbnDataListService)_provider.GetService(typeof(ITPM_YoyKbnDataListService));
            var reservationlst = await TPM_YoyKbnDataService.GetYoyKbnbySiyoKbn();
            BusCoordinationSearchParam reportParams = new BusCoordinationSearchParam();
            DateTime dateTimeConvert = DateTime.ParseExact(UnkobiDate, "yyyyMMdd", new CultureInfo("ja-JP"));
            if (string.IsNullOrEmpty(UnkobiDate))
            {
                dateTimeConvert = await GetUnkobiDate(Ukeno, UnkRen);
            }
            reportParams.StartDate = dateTimeConvert;
            reportParams.EndDate = dateTimeConvert;
            reportParams.BookingFrom = Ukeno;
            reportParams.BookingTo = Ukeno;
            reportParams.UnkRen = short.Parse(UnkRen);
            reportParams.ReservationList = reservationlst.Select(t=>t.YoyaKbnSeq).ToList();
            reportParams.DateType = 1;
            reportParams.OutputSetting = OutputInstruction.Pdf;
            List<BusCoordinationReportPDF> data = await GetPDFData(reportParams);
            if (data.Count > 0)
            {
                var report = await _reportLayoutSettingService.GetCurrentTemplate(ReportIdForSetting.Buscoordination, BaseNamespace.Buscoordination, new ClaimModel().TenantID, new ClaimModel().EigyoCdSeq, (byte)PaperSize.A4);
                report.DataSource = data;
                await new System.Threading.Tasks.TaskFactory().StartNew(() =>
                {
                    report.CreateDocument();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        /*if (type == 1)
                        {
                            PrintToolBase tool = new PrintToolBase(report.PrintingSystem);
                            tool.Print();
                            return;
                        }*/
                        report.ExportToPdf(ms);
                        byte[] exportedFileBytes = ms.ToArray();
                        string myExportString = Convert.ToBase64String(exportedFileBytes);
                        JSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, "pdf", "Busutehaisyo");
                    }
                });
            }
        }

        public async Task<DateTime> GetUnkobiDate(string Ukeno, string UnkRen)
        {
            short.TryParse(UnkRen, out short unkRen);
            var unkobi = await _mediator.Send(new GetTkdUnkobiByIdQuery() { Id = Ukeno, UnkRen = unkRen });
            DateTime result = DateTime.ParseExact(unkobi.HaiSymd, "yyyyMMdd", null);
            return result;
        }
        public async Task<DatetimeData> GetUnkobiDatebyUkenoList(string UkenoList)
        {
            var datetimeData = await _mediator.Send(new GetTkdUnkobibyUkenoListQuery() { UkenoList =  UkenoList});
            return datetimeData;
        }
    }
}
