using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Application.BillPrint.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.BillPrint;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Reports.DataSource;
using MediatR;

namespace HassyaAllrightCloud.IService
{
    public interface IBillPrintService
    {
        Task<List<DropDown>> GetReservationsAsync();
        Task<List<DropDown>> GetBillingOfficeDatasAsync();
        Task<List<DropDown>> GetBillingAddressesAsync();
        Task<List<PaymentRequestReport>> GetPaymentRequestAsync(BillPrintInput billPrintInput);
        Task<List<OutDataTableOutput>> GetOutDataTableAsync(string ukeNo);
        Task<PaymentRequestTenantInfo> GetTenantInfoAsync();
        Task<string> GetSeiFileIdAsync(int seiOutSeq, short seiRen);
        Task<string> SaveFileIdToTKDSeiKyuAsync(List<PaymentRequestGroup> paymentRequestGroups);
        Task<string> GetCompanyFileId();
    }

    public class BillPrintService : IBillPrintService, IReportService
    {
        private IMediator mediatR;
        private IFilterCondition FilterConditionService;
        private IGenerateFilterValueDictionary GenerateFilterValueDictionaryService;
        private IReportLayoutSettingService _reportLayoutSettingService;

        public BillPrintService(IMediator mediatR, IFilterCondition FilterConditionService, IGenerateFilterValueDictionary GenerateFilterValueDictionaryService,
            IReportLayoutSettingService reportLayoutSettingService)
        {
            this.mediatR = mediatR;
            this.FilterConditionService = FilterConditionService;
            this.GenerateFilterValueDictionaryService = GenerateFilterValueDictionaryService;
            _reportLayoutSettingService = reportLayoutSettingService;
        }

        public async Task<List<DropDown>> GetReservationsAsync()
        {
            return await mediatR.Send(new GetReservationsAsyncQuery { });
        }

        public async Task<List<DropDown>> GetBillingOfficeDatasAsync()
        {
            return await mediatR.Send(new GetBillingOfficeDataAsyncQuery { });
        }

        public async Task<List<DropDown>> GetBillingAddressesAsync()
        {
            return await mediatR.Send(new GetBillingAddressesAsyncQuery { });
        }

        public async Task<List<PaymentRequestReport>> GetPaymentRequestAsync(BillPrintInput billPrintInput)
        {
            return await mediatR.Send(new GetPaymentRequestAsyncQuery { billPrintInput = billPrintInput });
        }

        public async Task<List<OutDataTableOutput>> GetOutDataTableAsync(string ukeNo)
        {
            return await mediatR.Send(new GetOutDataTableAsyncQuery { ukeNo = ukeNo });
        }

        public async Task<PaymentRequestTenantInfo> GetTenantInfoAsync()
        {
            return await mediatR.Send(new GetTenantInfoAsyncQuery { });
        }

        public async Task<string> GetSeiFileIdAsync(int seiOutSeq, short seiRen)
        {
            return await mediatR.Send(new GetSeiFileIdAsyncQuery { seiOutSeq = seiOutSeq, seiRen = seiRen });
        }

        public async Task<XtraReport> PreviewReport(string queryParams)
        {
            var searchParams = EncryptHelper.DecryptFromUrl<BillPrintInput>(queryParams);
            var report = await _reportLayoutSettingService.GetCurrentTemplate(ReportIdForSetting.Billprint, BaseNamespace.Billprint, new ClaimModel().TenantID, new ClaimModel().EigyoCdSeq, (byte)PaperSize.A4);
            ObjectDataSource dataSource = new ObjectDataSource();
            var data = await GetPaymentRequestAsync(searchParams);
            if(data.Any())
            {
                Dictionary<string, string> keyValueFilterPairs = new Dictionary<string, string>();
                keyValueFilterPairs = await GenerateFilterValueDictionaryService.GenerateForBillPrint(searchParams);
                await FilterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.BillPrint, 0, new ClaimModel().SyainCdSeq);
            }
            Parameter param = new Parameter()
            {
                Name = "data",
                Type = typeof(List<PaymentRequestReport>),
                Value = data
            };
            dataSource.Name = "objectDataSource1";
            dataSource.DataSource = typeof(PaymentRequestReportDS);
            dataSource.Constructor = new ObjectConstructorInfo(param);
            dataSource.DataMember = "_data";
            report.DataSource = dataSource;
            report.BeforePrint += (sender, e) =>
            {
                var temp = sender as XtraReport;
                var lastCarryFowardAmountLabel = temp.AllControls<XRLabel>().Where(_ => _.Name.Contains("LastCarryFowardAmountLabel")).FirstOrDefault();
                var lastCarryFowardAmountValue = temp.AllControls<XRLabel>().Where(_ => _.Name.Contains("LastCarryFowardAmountValue")).FirstOrDefault();
                var lotsMaterialLabel = temp.AllControls<XRLabel>().Where(_ => _.Name.Contains("LotsMaterialLabel")).FirstOrDefault();
                var lotsMaterialValue = temp.AllControls<XRLabel>().Where(_ => _.Name.Contains("LotsMaterialValue")).FirstOrDefault();

                lastCarryFowardAmountLabel.Visible = data.Any() ? data.FirstOrDefault().MainInfoReport.ZenKurG != 0 : true;
                lastCarryFowardAmountValue.Visible = data.Any() ? data.FirstOrDefault().MainInfoReport.ZenKurG != 0 : true;
                lotsMaterialLabel.Visible = data.Any() ? data.FirstOrDefault().MainInfoReport.KonTesG != 0 : true;
                lotsMaterialValue.Visible = data.Any() ? data.FirstOrDefault().MainInfoReport.KonTesG != 0 : true;
            };
            return report;
        }

        public async Task<string> SaveFileIdToTKDSeiKyuAsync(List<PaymentRequestGroup> paymentRequestGroups)
        {
            return await mediatR.Send(new SaveFileIdToTKDSeiKyuAsyncQuery { paymentRequestGroups = paymentRequestGroups });
        }

        public async Task<string> GetCompanyFileId()
        {
            var company = await mediatR.Send(new GetCurrentCompany());
            return company != null ? company.ComSealImgFileId : string.Empty;
        }
    }
}
