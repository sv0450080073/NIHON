using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using System;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class AccessoryFeeListData
    {
        public int _ukeCdFrom { get; set; } = -1;
        public string UkeCdFrom
        {
            get
            {
                if (_ukeCdFrom == -1)
                    return string.Empty;
                return _ukeCdFrom.ToString("D10");
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _ukeCdFrom = -1;
                }
                else if (value.IsIntLargerThanZero())
                {
                    _ukeCdFrom = int.Parse(value);
                }
            }
        }

        public int _ukeCdTo { get; set; } = -1;
        public string UkeCdTo
        {
            get
            {
                if (_ukeCdTo == -1)
                    return string.Empty;
                return _ukeCdTo.ToString("D10");
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _ukeCdTo = -1;
                }
                else if (value.IsIntLargerThanZero())
                {
                    _ukeCdTo = int.Parse(value);
                }
            }
        }

        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;
        public LoadCustomerList customerList { get; set; }
        public CustomerComponentGyosyaData SelectedGyosyaStart { get; set; }
        public CustomerComponentTokiskData SelectedTokiskStart { get; set; }
        public CustomerComponentTokiStData SelectedTokistStart { get; set; }
        public CustomerComponentGyosyaData SelectedGyosyaEnd { get; set; }
        public CustomerComponentTokiskData SelectedTokiskEnd { get; set; }
        public CustomerComponentTokiStData SelectedTokistEnd { get; set; }
        public CompanyData Company { get; set; }
        public LoadSaleBranch BranchStart { get; set; }
        public LoadSaleBranch BranchEnd { get; set; }
        public LoadFutai FutaiStart { get; set; }
        public LoadFutai FutaiEnd { get; set; }
        public ReservationClassComponentData BookingTypeStart { get; set; }
        public ReservationClassComponentData BookingTypeEnd { get; set; }
        public List<ReservationClassComponentData> BookingTypes { get; set; }

        public OutputReportType ExportType { get; set; } = OutputReportType.Preview;
        public DateType DateType { get; set; } = DateType.Dispatch;
        public string DateTypeText { get; set; }
        public CsvConfigOption CsvConfigOption { get; set; } = new CsvConfigOption();
        public PaperSize PaperSize { get; set; }
        public SelectedOption<BreakReportPage> BreakPage { get; set; }
        public SelectedOption<InvoiceTypeOption> InvoiceType { get; set; }
        public List<LoadFutaiType> FutaiFeeTypes { get; set; } = new List<LoadFutaiType>();
        public SelectedOption<ReportType> ReportType { get; set; }
    }
}
