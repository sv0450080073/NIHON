using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Commons.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Reports.DataSource;
using MediatR;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Application.ReceivableList.Queries;

namespace HassyaAllrightCloud.IService
{
    public interface IReceivableListService
    {
        Task<List<SelectedSaleBranchModel>> GetSelectedSaleBranches(ReceivableFilterModel receivableFilterModel, int tenantCdSeq, int companyCd);
        Task<List<SelectedPaymentAddressModel>> GetSelectedPaymentAddresses(ReceivableFilterModel receivableFilterModel, int tenantCdSeq, int companyCd);
        Task<(List<ReservationListDetaiGridDataModel>, ReceivablePaymentSummary, int)> GetReceivableGridData(ReceivableFilterModel receivableFilterModel, bool isGetSingle, int pageNum, int pageSize, int tenantCdSeq, int companyCd);
        Task<List<ReceivableListReportDataModel>> GetReceivableListReportDatas(ReceivableFilterModel receivableFilterModel, int tenantCdSeq, int companyCd);
        Task<List<BusinessPlanReceivableListReportDataModel>> GetBusinessPlanReceivableListReportDatas(ReceivableFilterModel receivableFilterModel, int tenantCdSeq, int companyCd);
        Task<List<ReceivableCSVDataModel>> GetReceivableListCSVDatas(ReceivableFilterModel receivableFilterModel, int tenantCdSeq, int companyCd);
        Task<List<BusinessPlanReceivableCSVDataModel>> GetBusinessPlanReceivableListCSVDatas(ReceivableFilterModel receivableFilterModel, int tenantCdSeq, int companyCd);
        Task<(List<BussinesPlanReceivableGridDataModel>, BusinessPlanReceivablePaymentSummary, int)> GetPlanReceivableGridDatas(ReceivableFilterModel receivableFilterModel, bool isGetSingle, int pageNum, int pageSize, int tenantCdSeq, int companyCd);
    }
    public class ReceivableListService : IReceivableListService, IReportService
    {
        private IMediator _mediator;
        public ReceivableListService(IMediator mediatR)
        {
            _mediator = mediatR;
        }

        public async Task<List<ReceivableListReportDataModel>> GetReceivableListReportDatas(ReceivableFilterModel receivableFilterModel, int tenantCdSeq, int companyCd)
        {
            List<ReceivableListReportDataModel> result = new List<ReceivableListReportDataModel>();

            var receivableGridData = GetReceivableGridData(receivableFilterModel, false, 0, 100000, tenantCdSeq, companyCd).Result.Item1;
            var data = new ReceivableListReportDataModel();

            data.BillingCode = receivableFilterModel.SelectedBillingAddressPayment.Name;
            data.BillingCodeRange = receivableFilterModel?.startCustomerComponentTokiskData?.Text + receivableFilterModel?.startCustomerComponentTokiStData?.Text + "～" + receivableFilterModel?.endCustomerComponentTokiskData?.Text + receivableFilterModel?.endCustomerComponentTokiStData?.Text;
            data.BillingOffice = receivableFilterModel.SaleOfficeType.SaleOfficeName;
            data.BillingPeriod = receivableFilterModel.StartPaymentDate?.ToString() + "～" + receivableFilterModel.EndPaymentDate?.ToString();
            data.OutputDate = DateTime.Now;
            data.EmployeeCodeOfOutputPerson = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCd.ToString();
            data.OutputPersonEmployeeName = new HassyaAllrightCloud.Domain.Dto.ClaimModel().Name;
            data.SalesOfficeCodeRange = receivableFilterModel.StartSaleBranchList?.Text + "～" + receivableFilterModel.EndSaleBranchList?.Text;
            data.ReservationClassification = receivableFilterModel.StartReservationClassification?.Text + "～" + receivableFilterModel.EndReservationClassification?.Text;
            data.SpecifyPayment = receivableFilterModel.PaymentDate?.ToString().Substring(0,10);
            data.UnpaidDesignation = receivableFilterModel.UnpaidSelection?.StringValue;
            
            data.reservationListDetaiGridDatas = new List<ReservationListDetaiGridDataModel>();
            data.reservationListDetaiGridDatas = receivableGridData;
            result.Add(data);
            return result;
        }

        public async Task<(List<ReservationListDetaiGridDataModel>, ReceivablePaymentSummary, int)> GetReceivableGridData(ReceivableFilterModel receivableFilterModel, bool isGetSingle, int pageNum, int pageSize, int tenantCdSeq, int companyCd)
        {
            List<ReservationListDetaiGridDataModel> result = new List<ReservationListDetaiGridDataModel>();
            var dataFromQuery = await _mediator.Send(new GetReceivableList() {
                
                IsGetSingle = isGetSingle,
                PageNum = pageNum,
                PageSize = pageSize,
                ReceivableFilterModel = receivableFilterModel,
                CompanyCd = companyCd,
                TenantCdSeq = tenantCdSeq
            });
            int count = pageNum * pageSize;
            foreach(var item in dataFromQuery.Item1)
            {
                result.Add(new ReservationListDetaiGridDataModel()
                {
                    No = (count + 1).ToString(),
                    ReceiptNumber = item.UkeNo.Length >= 15 ? item.UkeNo.Substring(5) : item.UkeNo,
                    CustomerName = item.SeiRyakuNm +" "+item.SeiSitRyakuNm,
                    BillingDate = item.SeiTaiYmd != null ? item.SeiTaiYmd : string.Empty,
                    OperationScheduleSerialNumber = string.IsNullOrWhiteSpace(item.FutuUnkRenChar) || item.FutuUnkRenChar.Equals("0") ? string.Empty : item.FutuUnkRenChar.PadLeft(3, '0'),
                    GroupName = item.DanTaNm,
                    DestinationName = item.IkNm,
                    DeliveryDate = item.HaiSYmd != null ? item.HaiSYmd : string.Empty,
                    ArrivalDate = item.TouYmd != null ? item.TouYmd : string.Empty,
                    BillingType = item.SeiFutSyuNm,
                    LodingGoodsName = item.FutTumNm,
                    PaymentName = item.SeisanNm,
                    Quantity = item.Suryo,
                    UnitPrice = item.TanKa,
                    SalesAmount = item.UriGakKin,
                    TaxRate = item.Zeiritsu.ToString() + "%",
                    TaxAmount = item.SyaRyoSyo,
                    FeeRate = item.TesuRitu.ToString() + "%",
                    FeeAmount = item.SyaRyoTes,
                    BillingAmount = item.SeiKin,
                    DepositAmount = item.NyukinG,
                    UnpaidAmount = item.SeiKin - item.NyukinG - item.FuriTes,
                    CouponAmount = item.CouKesRui,
                    ReceptionOffice = item.EigyoRyak,
                    SeiRyakuNm = item.SeiRyakuNm,
                    SeiSitRyakuNm = item.SeiSitRyakuNm,
                    NyuKinKbn = Int32.Parse(item.NyuKinKbn),
                    NCouKbn = Int32.Parse(item.NCouKbn),
                    CumulativeDeposit = item.NyukinG + item.FuriTes,
                    YouKbn = item.YouKbn,
                    FutTumRen = item.FutTumRen,
                    FutuUnkRen = item.FutuUnkRen,
                    SeiFutSyu = item.SeiFutSyu,
                    UkeNo = item.UkeNo
                });
                count++;
            }
            return (result, dataFromQuery.Item2,dataFromQuery.Item3);
        }
        public async Task<List<SelectedPaymentAddressModel>> GetSelectedPaymentAddresses(ReceivableFilterModel receivableFilterModel, int tenantCdSeq, int companyCd)
        {
            return await _mediator.Send(new GetSelectedPaymentAddress() {
                CompanyCd = companyCd,
                TenantCdSeq = tenantCdSeq,
                ReceivableFilterModel = receivableFilterModel
            });
        }
        public async Task<List<SelectedSaleBranchModel>> GetSelectedSaleBranches(ReceivableFilterModel receivableFilterModel, int tenantCdSeq, int companyCd)
        {
            return await _mediator.Send(new GetSelectedSaleBranch() {
                CompanyCd = companyCd,
                TenantCdSeq = tenantCdSeq,
                ReceivableFilterModel = receivableFilterModel
            });
        }
        public async Task<List<ReceivableCSVDataModel>> GetReceivableListCSVDatas(ReceivableFilterModel receivableFilterModel, int tenantCdSeq, int companyCd)
        {
            List<ReceivableCSVDataModel> result = new List<ReceivableCSVDataModel>();
            var dataFromQuery = await _mediator.Send(new GetReceivableList()
            {
                CompanyCd = companyCd,
                TenantCdSeq = tenantCdSeq,
                IsGetSingle = false,
                ReceivableFilterModel = receivableFilterModel,
                PageNum = 0,
                PageSize = 1000000
            });

            foreach (var item in dataFromQuery.Item1)
            {
                result.Add(new ReceivableCSVDataModel()
                {
                    EigyoCd = item.EigyoCd,
                    EigyoNm = item.EigyoNm,
                    EigyoRyak = item.EigyoRyak,
                    SeiGyosyaCd = item.SeiGyosyaCd,
                    SeiCd = item.SeiCd,
                    SeiSitenCd = item.SeiSitenCd,
                    SeiGyosyaCdNm = item.SeiGyosyaCdNm,
                    SeiCdNm = item.SeiCdNm,
                    SeiSitenCdNm = item.SeiSitenCdNm,
                    SeiRyakuNm = item.SeiRyakuNm,
                    SeiSitRyakuNm = item.SeiSitRyakuNm,
                    UkeNo = item.UkeNo.Length >= 15 ? item.UkeNo.Substring(5) : item.UkeNo,
                    UkeEigyoCd = item.UkeEigyoCd,
                    UkeEigyoNm = item.UkeEigyoNm,
                    UkeRyakuNm = item.UkeEigyoNm,
                    GyosyaCd = item.GyosyaCd,
                    TokuiCd = item.TokuiCd,
                    SitenCd = item.SitenCd,
                    GyosyaNm = item.GyosyaNm,
                    TokuiNm = item.TokuiNm,
                    SitenNm = item.SitenNm,
                    TokRyakuNm = item.TokRyakuNm,
                    SitRyakuNm = item.SitRyakuNm,
                    SeiTaiYmd = item.SeiTaiYmd,
                    FutTumRen = item.FutTumRen.ToString(),
                    SeiFutSyu = item.SeiFutSyu.ToString(),
                    SeiFutSyuNm = item.SeiFutSyuNm,
                    DanTaNm = item.DanTaNm,
                    IkNm = item.IkNm,
                    HaiSYmd = item.HaiSYmd,
                    TouYmd = item.TouYmd,
                    FutTumKbn = item.FutTumKbn,
                    FutTumNm = item.FutTumNm,
                    SeisanCd = item.SeisanCd,
                    SeisanNm = item.SeisanNm,
                    Suryo = item.Suryo.ToString(),
                    TanKa = item.TanKa.ToString(),
                    UriGakKin = item.UriGakKin,
                    ZeiKbn = item.ZeiKbn,
                    ZeiKbnNm = item.ZeiKbnNm,
                    Zeiritsu = item.Zeiritsu.ToString(),
                    SyaRyoSyo = item.SyaRyoSyo,
                    TesuRitu = item.TesuRitu.ToString(),
                    SyaRyoTes = item.SyaRyoTes,
                    SeiKin = item.SeiKin,
                    NyukinGFuriTes = item.NyukinG + item.FuriTes,
                    SeiKinNyukinGFuriTes = item.FuriTes + item.NyukinG + item.SeiKin,
                    CouKesRui = item.CouKesRui,
                    TSiyoStaYmd = item.TSiyoStaYmd,
                    TSiyoEndYmd = item.TSiyoEndYmd,
                    SSiyoStaYmd = item.SSiyoStaYmd,
                    SSiyoEndYmd = item.SSiyoEndYmd,
                    YouKbn = item.YouKbn
                });
            }

            return result;
        }

        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            var searchParams = EncryptHelper.DecryptFromUrl<ReceivableFilterModel>(queryParams);
            XtraReport report = new XtraReport();

            if (searchParams.ReceivableReport == 1)
            {
                report = new Reports.ReceivableListReportA4();
                if (searchParams.ReportPageSize.IdValue == BillTypePagePrintList.BillTypePagePrintData[1].IdValue)
                {
                    report = new Reports.ReceivableListReportA3();
                }
                else
                {
                    if (searchParams.ReportPageSize.IdValue == BillTypePagePrintList.BillTypePagePrintData[2].IdValue)
                    {
                        report = new Reports.ReceivableListReportB4();
                    }
                }
            }
            else
            {
                report = new Reports.BusinessPlanReceivableListReportA4();
                if (searchParams.ReportPageSize.IdValue == BillTypePagePrintList.BillTypePagePrintData[1].IdValue)
                {
                    report = new Reports.BusinessPlanReceivableListReportA3();
                }
                else
                {
                    if (searchParams.ReportPageSize.IdValue == BillTypePagePrintList.BillTypePagePrintData[2].IdValue)
                    {
                        report = new Reports.BusinessPlanReceivableListReportB4();
                    }
                }
            }

            ObjectDataSource dataSource = new ObjectDataSource();
            List<ReceivableListReportDataModel> data = new List<ReceivableListReportDataModel>();
            List<BusinessPlanReceivableListReportDataModel> businessPlanData = new List<BusinessPlanReceivableListReportDataModel>();
            if (searchParams.ReceivableReport == 1)
            {
                data = await GetReceivableListReportDatas(searchParams, new ClaimModel().TenantID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID);
            }
            else
            {
                businessPlanData = await GetBusinessPlanReceivableListReportDatas(searchParams, new ClaimModel().TenantID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID);
            }
            dataSource.Name = "objectDataSource1";
            Parameter param = new Parameter()
            {
                Name = "data",
                Type = typeof(List<ReceivableListReportDataModel>),
                Value = data
            };
            Parameter businessPlanParam = new Parameter()
            {
                Name = "data",
                Type = typeof(List<BusinessPlanReceivableListReportDataModel>),
                Value = businessPlanData
            };
            if (searchParams.ReceivableReport == 1)
            {
                dataSource.DataSource = typeof(ReceivableListReportDS);
                dataSource.Constructor = new ObjectConstructorInfo(param);
            }
            else
            {
                dataSource.DataSource = typeof(BusinessPlanReceivableReportDS);
                dataSource.Constructor = new ObjectConstructorInfo(businessPlanParam);
            }
            dataSource.DataMember = "_data";

            report.DataSource = dataSource;
            return report;
        }

        public async Task<(List<BussinesPlanReceivableGridDataModel>, BusinessPlanReceivablePaymentSummary, int)> GetPlanReceivableGridDatas(ReceivableFilterModel receivableFilterModel, bool isGetSingle, int pageNum, int pageSize, int tenantCdSeq, int companyCd)
        {
            return await _mediator.Send(new GetBusinessPlanReceivableList()
            {
                CompanyCd = companyCd,
                TenantCdSeq = tenantCdSeq,
                IsGetSingle = isGetSingle,
                PageNum = pageNum,
                PageSize = pageSize,
                ReceivableFilterModel = receivableFilterModel,
                isReport = false
            });
        }

        public async Task<List<BusinessPlanReceivableListReportDataModel>> GetBusinessPlanReceivableListReportDatas(ReceivableFilterModel receivableFilterModel, int tenantCdSeq, int companyCd)
        {
            List<BusinessPlanReceivableListReportDataModel> result = new List<BusinessPlanReceivableListReportDataModel>();
            var businessPlanReceivableGridData = GetPlanReceivableGridDatas(receivableFilterModel, false, 0, 100000, tenantCdSeq, companyCd).Result.Item1;
            var data = new BusinessPlanReceivableListReportDataModel();

            data.BillingCode = receivableFilterModel.SelectedBillingAddressPayment.Name;
            data.BillingOffice = receivableFilterModel.SaleOfficeType.SaleOfficeName;
            data.BillingPeriod = receivableFilterModel.StartPaymentDate?.ToString() + "～" + receivableFilterModel.EndPaymentDate?.ToString();
            data.OutputDate = DateTime.Now;
            data.EmployeeCodeOfOutputPerson = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCd.ToString();
            data.OutputPersonEmployeeName = new HassyaAllrightCloud.Domain.Dto.ClaimModel().Name;
            data.ReservationClassification = receivableFilterModel?.startCustomerComponentTokiskData?.Text + receivableFilterModel?.startCustomerComponentTokiStData?.Text +"～" + receivableFilterModel?.endCustomerComponentTokiskData?.Text + receivableFilterModel?.endCustomerComponentTokiStData?.Text;
            data.SpecifyPayment = receivableFilterModel.PaymentDate?.ToString();
            data.UnpaidDesignation = receivableFilterModel.UnpaidSelection?.StringValue;
            data.CompanyCode = receivableFilterModel.CompanyData?.CompanyNm;
            data.OfficeCode = receivableFilterModel.SelectedBillingAddressPayment.CGyosyaNm;
            data.bussinesPlanReceivableGridDatas = new List<BussinesPlanReceivableGridDataModel>();
            data.bussinesPlanReceivableGridDatas = businessPlanReceivableGridData;

            result.Add(data);

            return result;
        }

        public async Task<List<BusinessPlanReceivableCSVDataModel>> GetBusinessPlanReceivableListCSVDatas(ReceivableFilterModel receivableFilterModel, int tenantCdSeq, int companyCd)
        {
            List<BusinessPlanReceivableCSVDataModel> result = new List<BusinessPlanReceivableCSVDataModel>();
            var dataFromQuery = await _mediator.Send(new GetBusinessPlanReceivableList()
            {
                CompanyCd = companyCd,
                TenantCdSeq = tenantCdSeq,
                ReceivableFilterModel = receivableFilterModel,
                IsGetSingle = false,
                PageNum = 0,
                PageSize = 1000000,
                isReport = true
            });

            foreach (var item in dataFromQuery.Item1)
            {
                result.Add(new BusinessPlanReceivableCSVDataModel()
                {
                    EigyoCd = item.EigyoCd,
                    EigyoNm = item.EigyoNm,
                    EigyoRyak = item.EigyoRyak,
                    SeiGyosyaCd = item.SeiGyosyaCd,
                    SeiCd = item.SeiCd,
                    SeiSitenCd = item.SeiSitenCd,
                    SeiGyosyaCdNm = item.SeiGyosyaCdNm,
                    SeiCdNm = item.SeiCdNm,
                    SeiSitenCdNm = item.SeiSitenCdNm,
                    SeiRyakuNm = item.SeiRyakuNm,
                    SeiSitRyakuNm = item.SeiSitRyakuNm,
                    UnUriGakKin = item.UnUriGakKin,
                    UnSyaRyoSyo = item.UnSyaRyoSyo,
                    UnSyaRyoTes = item.UnSyaRyoTes,
                    UnNyukinG = item.UnNyukinG,
                    UnMisyuG = item.UnMisyuG,
                    GaUriGakKin = item.GaUriGakKin,
                    GaSyaRyoSyo = item.GaSyaRyoSyo,
                    GaSyaRyoTes = item.GaSyaRyoTes,
                    GaNyukinG = item.GaNyukinG,
                    GaMisyuG = item.GaMisyuG,
                    SoUriGakKin = item.SoUriGakKin,
                    SoSyaRyoSyo = item.SoSyaRyoSyo,
                    SoSyaRyoTes = item.SoSyaRyoTes,
                    SoNyukinG = item.SoNyukinG,
                    SoMisyuG = item.SoMisyuG,
                    CaUriGakKin = item.CaUriGakKin,
                    CaSyaRyoSyo = item.CaSyaRyoSyo,
                    CaNyukinG = item.CaNyukinG,
                    CaMisyuG = item.CaMisyuG,
                    MisyuG = item.MisyuG
                });
            }

            return result;
        }
    }
}
