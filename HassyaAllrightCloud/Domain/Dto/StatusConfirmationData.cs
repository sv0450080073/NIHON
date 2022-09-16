using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using System;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class StatusConfirmationData
    {
        /// <summary>
        /// Get/set start date to filter. Default is Today
        /// </summary>
        public DateTime StartDate { get; set; } = DateTime.Today;
        /// <summary>
        /// Get/set end date to filter. Default is Today + 1
        /// </summary>
        public DateTime EndDate { get; set; } = DateTime.Today.AddDays(1);
        public CompanyData SelectedCompany { get; set; }
        public LoadSaleBranch BranchStart { get; set; } 
        public LoadSaleBranch BranchEnd { get; set; }
        // N.T.L.Anh Add STR 2021/06/07
        //public LoadCustomerList CustomerStart { get; set; } 
        //public LoadCustomerList CustomerEnd { get; set; } 
        public CustomerComponentGyosyaData GyosyaTokuiSakiFrom { get; set; }
        public CustomerComponentGyosyaData GyosyaTokuiSakiTo { get; set; }
        public CustomerComponentTokiskData TokiskTokuiSakiFrom { get; set; }
        public CustomerComponentTokiskData TokiskTokuiSakiTo { get; set; }
        public CustomerComponentTokiStData TokiStTokuiSakiFrom { get; set; }
        public CustomerComponentTokiStData TokiStTokuiSakiTo { get; set; }
        // N.T.L.Anh Add END 2021/06/07
        public ConfirmStatus ConfirmedStatus { get; set; } = ConfirmStatus.Confirmed;
        public ConfirmStatus FixedStatus { get; set; } = ConfirmStatus.Fixed;
        public SelectedOption<NumberOfConfirmed> ConfirmedTimes { get; set; }
        public SelectedOption<ConfirmStatus> Saikou { get; set; }
        public SelectedOption<ConfirmStatus> SumDai { get; set; }
        public SelectedOption<ConfirmStatus> Ammount { get; set; }
        public SelectedOption<ConfirmStatus> ScheduleDate { get; set; }
        public OutputReportType ExportType { get; set; } = OutputReportType.Preview;
        public PaperSize PaperSize { get; set; } = PaperSize.A3;
        public CsvConfigOption CsvConfigOption { get; set; } = new CsvConfigOption();
        public int Size { get; set; } = (int)ViewMode.Large;

        public static StatusConfirmationData ApplySelectAllFromNull(StatusConfirmationData dataSource)
        {
            var result = new StatusConfirmationData();

            result.SimpleCloneProperties(dataSource);

            result.GyosyaTokuiSakiFrom = result.GyosyaTokuiSakiFrom is null
                ? new CustomerComponentGyosyaData() { GyosyaCdSeq = 0 }
                : dataSource.GyosyaTokuiSakiFrom;
            result.GyosyaTokuiSakiTo = result.GyosyaTokuiSakiTo is null
                ? new CustomerComponentGyosyaData() { GyosyaCdSeq = 0 }
                : dataSource.GyosyaTokuiSakiTo;

            result.SelectedCompany = result.SelectedCompany is null
                ? new CompanyData() { IsSelectedAll = true }
                : dataSource.SelectedCompany;

            result.BranchStart = result.BranchStart is null
                ? new LoadSaleBranch() { IsSelectedAll = true }
                : dataSource.BranchStart;
            result.BranchEnd = result.BranchEnd is null
                ? new LoadSaleBranch() { IsSelectedAll = true }
                : dataSource.BranchEnd;

            return result;
        }
    }

}
