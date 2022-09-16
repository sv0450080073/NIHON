using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using MediatR;
using HassyaAllrightCloud.Application.DepositList.Queries;
using HassyaAllrightCloud.Commons.Constants;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Reports.DataSource;
using DevExpress.DataAccess.ObjectBinding;
using System.Globalization;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using HassyaAllrightCloud.IService.CommonComponents;

namespace HassyaAllrightCloud.IService
{
    public interface IDepositListService
    {
        Task<List<CompanyData>> GetCompanyData(int tenantCdSeq);
        Task<List<LoadSaleBranchList>> GetSaleBranchData(int tenantCdSeq);
        List<SaleOfficeModel> GetSaleOfficeModel();
        Task<List<BillingAddressModel>> GetBillingAddresses(int tenantCdSeq);
        Task<List<ReservationCategoryModel>> GetReservationCategories(int tenantCdSeq);
        Task<List<TransferBankModel>> GetTransferBanks();
        Task<CharterSettingModel> GetCharterSetting(int companyCdSeq);
        Task<List<SelectedSaleBranchModel>> GetSelectedSaleBranches(DepositListSearchModel depositListSearchModel, int tenantCdSeq);
        Task<List<SelectedPaymentAddressModel>> GetSelectedPaymentAddresses(DepositListSearchModel depositListSearchModel, int tenantCdSeq);
        Task<(List<DepositDataGrid>, DepositListSummary, int)> GetDepositList(DepositListSearchModel depositListSearchModel, bool ssGetSingle,  int tenantCdSeq, int companyCdSeq);
        Task<List<DepositsListReportDataModel>> GetDepositsListReportDatas(DepositListSearchModel depositListSearchModel, int tenantCdSeq, int companyCdSeq);
        Task<List<DepositListCSVDataModel>> GetDepositListCSVDatas(DepositListSearchModel depositListSearchModel, int tenantCdSeq, int companyCdSeq);
        Task<List<CustomerComponentGyosyaData>> GetListGyosya();
        Task<List<CustomerComponentTokiskData>> GetListTokisk();
        Task<List<CustomerComponentTokiStData>> GetListTokiSt();
        Task<List<ReservationClassComponentData>> GetListReservationClass();
    }
    public class DepositListService : IDepositListService, IReportService
    {
        private IMediator _mediator;
        public DepositListService(IMediator mediatR)
        {
            _mediator = mediatR;
        }

        public async Task<List<BillingAddressModel>> GetBillingAddresses(int tenantCdSeq)
        {
            return await _mediator.Send(new GetBillingAddresses() { TenantCdSeq = tenantCdSeq });
        }

        public async Task<CharterSettingModel> GetCharterSetting(int companyCdSeq)
        {
            return await _mediator.Send(new GetCharterSetting() { CompanyCdSeq = companyCdSeq });
        }

        public async Task<List<CompanyData>> GetCompanyData(int tenantCdSeq)
        {
            return await _mediator.Send(new GetCompanyData() { TenantCdSeq = tenantCdSeq });
        }

        public async Task<(List<DepositDataGrid>, DepositListSummary, int)> GetDepositList(DepositListSearchModel depositListSearchModel, bool isGetSingle, int tenantCdSeq, int companyCdSeq)
        {
            int SeiKrksKbn = GetCharterSetting(companyCdSeq).Result.SeiKrksKbn;
            string SyoriYm = GetCharterSetting(companyCdSeq).Result.SyoriYm;
            return await _mediator.Send(new GetDepositList() { TenantCdSeq = tenantCdSeq, depositListSearchModel = depositListSearchModel, SeiKrksKbn = SeiKrksKbn, SyoriYm = SyoriYm, IsGetSingle = isGetSingle });
        }

        public async Task<List<DepositListCSVDataModel>> GetDepositListCSVDatas(DepositListSearchModel depositListSearchModel, int tenantCdSeq, int companyCdSeq)
        {
            int SeiKrksKbn = GetCharterSetting(companyCdSeq).Result.SeiKrksKbn;
            string SyoriYm = GetCharterSetting(companyCdSeq).Result.SyoriYm;
            string NyuSEtcNm1 = GetCharterSetting(companyCdSeq).Result.NyuSEtcNm1;
            string NyuSEtcNm2 = GetCharterSetting(companyCdSeq).Result.NyuSEtcNm2;
            List<DepositListCSVDataModel> result = await _mediator.Send(new GetDepositListCSVData() {SeiKrksKbn = SeiKrksKbn, SyoriYm = SyoriYm, TenantCdSeq = new ClaimModel().TenantID, depositListSearchModel = depositListSearchModel });
            foreach(var item in result)
            {
                item.NyuSEtcNm1 = NyuSEtcNm1;
                item.NyuSEtcNm2 = NyuSEtcNm2;
            }
            return result;
        }

        public async Task<List<DepositsListReportDataModel>> GetDepositsListReportDatas(DepositListSearchModel depositListSearchModel, int tenantCdSeq, int companyCdSeq)
        {
            var chacterSetting = GetCharterSetting(companyCdSeq).Result;
            List<DepositDataGrid> depositDataGrids = GetDepositList(depositListSearchModel, false,  tenantCdSeq, companyCdSeq).Result.Item1;
            DepositsListReportDataModel data = new DepositsListReportDataModel();
            List<DepositsListReportDataModel> result = new List<DepositsListReportDataModel>();
            data.OutputDate = DateTime.Now.ToString("yyyyMMdd").Insert(4, "/").Insert(7, "/") + " " + DateTime.Now.ToString("HHmmss").Substring(0,4).Insert(2, ":");
            data.Page = string.Empty;
            data.BillingOffice = depositListSearchModel.SelectedSaleBranchPayment == null ? string.Empty : depositListSearchModel.SelectedSaleBranchPayment.Text.Trim();
            data.BillingCode = depositListSearchModel.SelectedBillingAddressPayment == null ? string.Empty : depositListSearchModel.SelectedBillingAddressPayment.Text.Trim();
            data.PaymentDate = (depositListSearchModel.StartPaymentDate == null ? string.Empty : depositListSearchModel.StartPaymentDate.ToString().Substring(0, 10).Trim()) + " ～ " + (depositListSearchModel.EndPaymentDate == null ? string.Empty : depositListSearchModel.EndPaymentDate.ToString().Substring(0, 10).Trim());
            data.BillingCodeRange = (depositListSearchModel?.startCustomerComponentTokiskData == null ? string.Empty : depositListSearchModel.startCustomerComponentTokiskData.Text.Trim()) + (depositListSearchModel?.startCustomerComponentTokiStData == null ? string.Empty : depositListSearchModel.startCustomerComponentTokiStData.Text.Trim())
                + " ~ " +
                (depositListSearchModel?.endCustomerComponentTokiskData == null ? string.Empty : depositListSearchModel.endCustomerComponentTokiskData.Text.Trim()) + (depositListSearchModel?.endCustomerComponentTokiStData == null ? string.Empty : depositListSearchModel.endCustomerComponentTokiStData.Text.Trim());
            data.CompanyCode = depositListSearchModel.CompanyData == null ? string.Empty : depositListSearchModel.CompanyData.Text.Trim();
            data.SalesOfficeCodeRange = (depositListSearchModel.StartSaleBranchList == null ? string.Empty : depositListSearchModel.StartSaleBranchList.Text.Trim()) + " ~ " + (depositListSearchModel.EndSaleBranchList == null ? string.Empty : depositListSearchModel.EndSaleBranchList.Text.Trim());
            data.TransferBankHeader = (depositListSearchModel.StartTransferBank == null ? string.Empty : depositListSearchModel.StartTransferBank.Text.Trim()) + " ~ " + (depositListSearchModel.EndTransferBank == null ? string.Empty :depositListSearchModel.EndTransferBank.Text.Trim());
            data.OutputPersonCode = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCd;
            data.OutputPersonName = new HassyaAllrightCloud.Domain.Dto.ClaimModel().Name;
            data.NyuSEtcNm1 = chacterSetting.NyuSEtcNm1;
            data.NyuSEtcNm2 = chacterSetting.NyuSEtcNm2;
            data.NyuSyoNm11 = chacterSetting.NyuSyoNm11;
            data.NyuSyoNm12 = chacterSetting.NyuSyoNm12;
            data.NyuSyoNm21 = chacterSetting.NyuSyoNm21;
            data.NyuSyoNm22 = chacterSetting.NyuSyoNm22;
            data.DepositDatas = new List<DepositDataGrid>();
            foreach (var item in depositDataGrids)
            {
                var dataGridTemp = new DepositDataGrid()
                {
                    Amount = item.Amount,
                    AmountAndTransferFee = item.Amount + item.TransferFee,
                    TransferFee = item.TransferFee,
                    ArrivalDate = !string.IsNullOrWhiteSpace(item.ArrivalDate) ? item.ArrivalDate.Insert(4,"/").Insert(7,"/") : string.Empty,
                    BillingType = string.IsNullOrWhiteSpace(item.BillingType) ? item.BillingType : string.Empty,
                    CardApprovalNumber = item.CardApprovalNumber != null ? item.CardApprovalNumber.Replace(" ", string.Empty) : string.Empty,
                    CardSlipNumber = item.CardSlipNumber != null ? item.CardSlipNumber.Replace(" ", string.Empty) : string.Empty,
                    CouponFaceValue = item.CouponFaceValue != null ? item.CouponFaceValue.Replace(" ", string.Empty) : string.Empty,
                    CouponNo = item.CouponNo != null ? item.CouponNo.Replace(" ", string.Empty) : string.Empty,
                    CouponNoAndCouponFaceValue = item.CouponNo != null && item.CouponFaceValue != null ? item.CouponNo.Replace(" ", string.Empty) + item.CouponFaceValue.Replace(" ", string.Empty) : string.Empty,
                    CumulativePayment = item.CumulativePayment,
                    CumulativePaymentAndPreviousReceiveAmount = item.CumulativePayment + item.PreviousReceiveAmount,
                    PreviousReceiveAmount = item.PreviousReceiveAmount,
                    CustomerName = item.CustomerName != null ? item.CustomerName.Replace(" ", string.Empty) : string.Empty,
                    DeliveryDate = !string.IsNullOrWhiteSpace(item.DeliveryDate) ? item.DeliveryDate.Insert(4, "/").Insert(7, "/") : string.Empty,
                    DeliveryDateAndArrivalDate = item.DeliveryDate != null && item.ArrivalDate != null ?item.DeliveryDate.Replace(" ", string.Empty) + item.ArrivalDate.Replace(" ", string.Empty) : string.Empty,
                    DestinationName = item.DestinationName != null ? item.DestinationName.Replace(" ", string.Empty) : string.Empty,
                    FutTumNm = item.FutTumNm != null ? item.FutTumNm.Replace(" ", string.Empty) : string.Empty,
                    GroupName = item.GroupName != null ? item.GroupName.Replace(" ", string.Empty) : string.Empty,
                    GroupNameAndDestinationName = item.GroupName != null && item.DestinationName != null ? item.GroupName.Replace(" ", string.Empty) + item.DestinationName.Replace(" ", string.Empty) : string.Empty,
                    LoadingGoodName = item.LoadingGoodName != null ? item.LoadingGoodName.Replace(" ", string.Empty) : string.Empty,
                    No = item.No,
                    OperatingSerialNumber = item.OperatingSerialNumber != null ? item.OperatingSerialNumber.Replace(" ", string.Empty) : string.Empty,
                    Other11 = item.Other11 != null ? item.Other11.Replace(" ", string.Empty) : string.Empty,
                    Other12 = item.Other12 != null ? item.Other12.Replace(" ", string.Empty) : string.Empty,
                    Other21 = item.Other21 != null ? item.Other21.Replace(" ", string.Empty) : string.Empty,
                    Other22 = item.Other22 != null ? item.Other22.Replace(" ", string.Empty) : string.Empty,
                    PaperDueDate = item.PaperDueDate != null ? item.PaperDueDate : string.Empty,
                    PaperNumber = item.PaperNumber != null ? item.PaperNumber.Replace(" ", string.Empty) : string.Empty,
                    PaymenMethod = item.PaymenMethod != null ? item.PaymenMethod.Replace(" ", string.Empty) : string.Empty,
                    PaymentDate = !string.IsNullOrWhiteSpace(item.PaymentDate) ? item.PaymentDate.Insert(4, "/").Insert(7, "/") : string.Empty,
                    ReceptionOffice = item.ReceptionOffice != null ? item.ReceptionOffice.Replace(" ", string.Empty) : string.Empty,
                    Transferbank = item.Transferbank != null ? item.Transferbank.Replace(" ", string.Empty) : string.Empty,
                    UkeNo = item.UkeNo != null ? item.UkeNo.Replace(" ", string.Empty) : string.Empty,
                    UkeNoAndReceptionOffice = item.UkeNo != null && item.ReceptionOffice != null ? item.UkeNo.Replace(" ", string.Empty) + item.ReceptionOffice.Replace(" ", string.Empty) : string.Empty,
                    ServiceDate = item.ServiceDate,
                    SaleOffice = item.SaleOffice,
                    Cash = item.Cash,
                    Another = item.Another,
                    AdjustMoney = item.AdjustMoney,
                    Bill = item.Bill,
                    Compensation = item.Compensation,
                    BankNm = item.BankNm,
                    BankSitNm = item.BankSitNm,
                    Division = item.Division,
                    TransferAmount = item.TransferAmount
                };
                data.DepositDatas.Add(dataGridTemp);

            }
            result.Add(data);
            return result;
        }

        public async Task<List<CustomerComponentGyosyaData>> GetListGyosya()
        {
            return await _mediator.Send(new GetListGyosyaQuery());
        }

        public async Task<List<ReservationClassComponentData>> GetListReservationClass()
        {
            return await _mediator.Send(new GetListReservationClassQuery());
        }

        public async Task<List<CustomerComponentTokiskData>> GetListTokisk()
        {
            return await _mediator.Send(new GetListTokiskQuery());
        }

        public async Task<List<CustomerComponentTokiStData>> GetListTokiSt()
        {
            return await _mediator.Send(new GetListTokiStQuery());
        }

        public async Task<List<ReservationCategoryModel>> GetReservationCategories(int tenantCdSeq)
        {
            return await _mediator.Send(new GetReservationCategory() { TenantCdSeq = tenantCdSeq });
        }

        public async Task<List<LoadSaleBranchList>> GetSaleBranchData(int tenantCdSeq)
        {
            return await _mediator.Send(new GetSaleBranchData() { TenantCdSeq = tenantCdSeq });
        }

        public List<SaleOfficeModel> GetSaleOfficeModel()
        {
            List<SaleOfficeModel> result = new List<SaleOfficeModel>();
            result.Add(new SaleOfficeModel()
            {
                SaleOfficeKbn = 1,
                SaleOfficeName = DepositList.PaymentOffice
            });
            result.Add(new SaleOfficeModel()
            {
                SaleOfficeKbn = 2,
                SaleOfficeName = DepositList.ReceprtionOffice
            });
            result.Add(new SaleOfficeModel()
            {
                SaleOfficeKbn = 3,
                SaleOfficeName = DepositList.Depositoffice
            });
            return result;
        }

        public async Task<List<SelectedPaymentAddressModel>> GetSelectedPaymentAddresses(DepositListSearchModel depositListSearchModel, int tenantCdSeq)
        {
            return await _mediator.Send(new GetSelectedPaymentAddress() { TenantCdSeq = tenantCdSeq, depositListSearchModel = depositListSearchModel });
        }

        public async Task<List<SelectedSaleBranchModel>> GetSelectedSaleBranches(DepositListSearchModel depositListSearchModel, int tenantCdSeq)
        {
            return await _mediator.Send(new GetSelectedSaleBranch() { TenantCdSeq = tenantCdSeq, depositListSearchModel = depositListSearchModel});
        }

        public async Task<List<TransferBankModel>> GetTransferBanks()
        {
            return await _mediator.Send(new GetTransferBankModel());
        }

        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            var searchParams = EncryptHelper.DecryptFromUrl<DepositListSearchModel>(queryParams);
            XtraReport report = new XtraReport();

            if (searchParams.DepositOutputTemplate.Id == 1){
                report = new Reports.DepositListReportA4();
                if (searchParams.PageSizeReport.IdValue == BillTypePagePrintList.BillTypePagePrintData[1].IdValue)
                {
                    report = new Reports.DepositListReportA3();
                }
                else
                {
                    if (searchParams.PageSizeReport.IdValue == BillTypePagePrintList.BillTypePagePrintData[2].IdValue)
                    {
                        report = new Reports.DepositListReportB4();
                    }
                }
            }
            if(searchParams.DepositOutputTemplate.Id == 2)
            {
                report = new Reports.DepositOfficeListReportA4();
                if (searchParams.PageSizeReport.IdValue == BillTypePagePrintList.BillTypePagePrintData[1].IdValue)
                {
                    report = new Reports.DepositOfficeListReportA3();
                }
                else
                {
                    if (searchParams.PageSizeReport.IdValue == BillTypePagePrintList.BillTypePagePrintData[2].IdValue)
                    {
                        report = new Reports.DepositOfficeListReportB4();
                    }
                }
            }
            searchParams.IsGetAll = true;
            ObjectDataSource dataSource = new ObjectDataSource();
            var data = await GetDepositsListReportDatas(searchParams, new ClaimModel().TenantID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID);
            Parameter param = new Parameter()
            {
                Name = "data",
                Type = typeof(List<DepositsListReportDataModel>),
                Value = data
            };
            dataSource.Name = "objectDataSource1";
            dataSource.DataSource = typeof(DepositListReportDS);
            dataSource.Constructor = new ObjectConstructorInfo(param);
            dataSource.DataMember = "_data";
            report.DataSource = dataSource;
            return report;
        }
    }
}
