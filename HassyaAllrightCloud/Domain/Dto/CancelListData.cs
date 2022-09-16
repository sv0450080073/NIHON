using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using System;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class CancelListData
    {
        public static CancelListData ApplySelectAllFromNull(CancelListData dataSource)
        {
            var result = new CancelListData();

            result.UkeCdFrom = dataSource.UkeCdFrom;
            result.UkeCdTo = dataSource.UkeCdTo;
            result.DateType = dataSource.DateType;
            result.DateTypeText = dataSource.DateTypeText;
            result.Sort = dataSource.Sort;
            result.SortText = dataSource.SortText;
            result.YoyakuFrom = dataSource.YoyakuFrom;
            result.YoyakuTo = dataSource.YoyakuTo;
            result.StartDate = dataSource.StartDate;
            result.EndDate = dataSource.EndDate;
            result.GyosyaTokuiSakiFrom = dataSource.GyosyaTokuiSakiFrom;
            result.GyosyaTokuiSakiTo = dataSource.GyosyaTokuiSakiTo;
            result.TokiskTokuiSakiFrom = dataSource.TokiskTokuiSakiFrom;
            result.TokiskTokuiSakiTo = dataSource.TokiskTokuiSakiTo;
            result.TokiStTokuiSakiFrom = dataSource.TokiStTokuiSakiFrom;
            result.TokiStTokuiSakiTo = dataSource.TokiStTokuiSakiTo;
            result.GyosyaShiireSakiFrom = dataSource.GyosyaShiireSakiFrom;
            result.GyosyaShiireSakiTo = dataSource.GyosyaShiireSakiTo;
            result.TokiskShiireSakiFrom = dataSource.TokiskShiireSakiFrom;
            result.TokiskShiireSakiTo = dataSource.TokiskShiireSakiTo;
            result.TokiStShiireSakiFrom = dataSource.TokiStShiireSakiFrom;
            result.TokiStShiireSakiTo = dataSource.TokiStShiireSakiTo;
            result.CancelCharge = dataSource.CancelCharge;
            result.BreakPage = dataSource.BreakPage;
            result.ExportType = dataSource.ExportType;
            result.PaperSize = dataSource.PaperSize;
            result.CsvConfigOption = dataSource.CsvConfigOption;
            result.Size = dataSource.Size;
            result.BookingTypes = dataSource.BookingTypes;

            result.YoyakuFrom = result.YoyakuFrom is null
                ? new ReservationClassComponentData()
                : result.YoyakuFrom;
            result.YoyakuTo = result.YoyakuTo is null
                ? new ReservationClassComponentData()
                : result.YoyakuTo;

            result.Company = dataSource.Company is null
                ? new CompanyData() { IsSelectedAll = true }
                : dataSource.Company;

            result.CancelBookingType = dataSource.CancelBookingType is null
                ? new TPM_CodeKbData() { CodeKbnName = Constants.SelectedAll, CodeKb_CodeKbn = "00" }
                : dataSource.CancelBookingType;

            result.BranchStart = dataSource.BranchStart is null
                ? new LoadSaleBranch() { IsSelectedAll = true }
                : dataSource.BranchStart;
            result.BranchEnd = dataSource.BranchEnd is null
                ? new LoadSaleBranch() { IsSelectedAll = true }
                : dataSource.BranchEnd;

            result.StaffStart = dataSource.StaffStart is null
                ? new LoadStaff()
                : dataSource.StaffStart;
            result.StaffEnd = dataSource.StaffEnd is null
                ? new LoadStaff()
                : dataSource.StaffEnd;
            result.CancelStaffStart = dataSource.CancelStaffStart is null
                ? new LoadStaff()
                : dataSource.CancelStaffStart;
            result.CancelStaffEnd = dataSource.CancelStaffEnd is null
                ? new LoadStaff()
                : dataSource.CancelStaffEnd;
            
            result.GyosyaTokuiSakiFrom = dataSource.GyosyaTokuiSakiFrom is null
                ? new CustomerComponentGyosyaData { GyosyaCdSeq = 0 }
                : dataSource.GyosyaTokuiSakiFrom;
            result.GyosyaTokuiSakiTo = dataSource.GyosyaTokuiSakiTo is null
                ? new CustomerComponentGyosyaData { GyosyaCdSeq = 0 }
                : dataSource.GyosyaTokuiSakiTo;
            result.TokiskTokuiSakiFrom = dataSource.TokiskTokuiSakiFrom is null
                ? new CustomerComponentTokiskData { TokuiSeq = 0 }
                : dataSource.TokiskTokuiSakiFrom;
            result.TokiskTokuiSakiTo = dataSource.TokiskTokuiSakiTo is null
                ? new CustomerComponentTokiskData { TokuiSeq = 0 }
                : dataSource.TokiskTokuiSakiTo;
            result.TokiStTokuiSakiFrom = dataSource.TokiStTokuiSakiFrom is null
                ? new CustomerComponentTokiStData { SitenCdSeq = 0 }
                : dataSource.TokiStTokuiSakiFrom;
            result.TokiStTokuiSakiTo = dataSource.TokiStTokuiSakiTo is null
                ? new CustomerComponentTokiStData { SitenCdSeq = 0 }
                : dataSource.TokiStTokuiSakiTo;

            result.GyosyaShiireSakiFrom = dataSource.GyosyaShiireSakiFrom is null
                ? new CustomerComponentGyosyaData { GyosyaCdSeq = 0 }
                : dataSource.GyosyaShiireSakiFrom;
            result.GyosyaShiireSakiTo = dataSource.GyosyaShiireSakiTo is null
                ? new CustomerComponentGyosyaData { GyosyaCdSeq = 0 }
                : dataSource.GyosyaShiireSakiTo;
            result.TokiskShiireSakiFrom = dataSource.TokiskShiireSakiFrom is null
                ? new CustomerComponentTokiskData { TokuiSeq = 0 }
                : dataSource.TokiskShiireSakiFrom;
            result.TokiskShiireSakiTo = dataSource.TokiskShiireSakiTo is null
                ? new CustomerComponentTokiskData { TokuiSeq = 0 }
                : dataSource.TokiskShiireSakiTo;
            result.TokiStShiireSakiFrom = dataSource.TokiStShiireSakiFrom is null
                ? new CustomerComponentTokiStData { SitenCdSeq = 0 }
                : dataSource.TokiStShiireSakiFrom;
            result.TokiStShiireSakiTo = dataSource.TokiStShiireSakiTo is null
                ? new CustomerComponentTokiStData { SitenCdSeq = 0 }
                : dataSource.TokiStShiireSakiTo;


            return result;
        }

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

        public DateType DateType { get; set; } = DateType.Cancellation;
        public string DateTypeText { get; set; }
        public SortCancel Sort { get; set; } = SortCancel.Customer;
        public string SortText { get; set; }
        public List<ReservationClassComponentData> BookingTypes { get; set; }

        //Change item search
        public ReservationClassComponentData YoyakuFrom { get; set; }
        public ReservationClassComponentData YoyakuTo { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Today;
        public DateTime EndDate { get; set; } = DateTime.Today;
        public CompanyData Company { get; set; }
        public TPM_CodeKbData CancelBookingType { get; set; }
        public SelectedOption<ConfirmAction> CancelCharge { get; set; }
        public SelectedOption<BreakReportPage> BreakPage { get; set; }
        public LoadSaleBranch BranchStart { get; set; }
        public LoadSaleBranch BranchEnd { get; set; }

        public LoadCustomerList CustomerStart { get; set; }
        public LoadCustomerList CustomerEnd { get; set; }
        public LoadCustomerList SupplierStart { get; set; }
        public LoadCustomerList SupplierEnd { get; set; }

        // 得意先

        // New
        public CustomerComponentGyosyaData GyosyaTokuiSakiFrom { get; set; }
        public CustomerComponentGyosyaData GyosyaTokuiSakiTo { get; set; }
        public CustomerComponentTokiskData TokiskTokuiSakiFrom { get; set; }
        public CustomerComponentTokiskData TokiskTokuiSakiTo { get; set; }
        public CustomerComponentTokiStData TokiStTokuiSakiFrom { get; set; }
        public CustomerComponentTokiStData TokiStTokuiSakiTo { get; set; }

        // 仕入先

        // New
        public CustomerComponentGyosyaData GyosyaShiireSakiFrom { get; set; }
        public CustomerComponentGyosyaData GyosyaShiireSakiTo { get; set; }
        public CustomerComponentTokiskData TokiskShiireSakiFrom { get; set; }
        public CustomerComponentTokiskData TokiskShiireSakiTo { get; set; }
        public CustomerComponentTokiStData TokiStShiireSakiFrom { get; set; }
        public CustomerComponentTokiStData TokiStShiireSakiTo { get; set; }

        public LoadStaff StaffStart { get; set; }
        public LoadStaff StaffEnd { get; set; }
        public LoadStaff CancelStaffStart { get; set; }
        public LoadStaff CancelStaffEnd { get; set; }
        public OutputReportType ExportType { get; set; } = OutputReportType.Preview;
        public PaperSize PaperSize { get; set; }
        public CsvConfigOption CsvConfigOption { get; set; } = new CsvConfigOption();
        public int Size { get; set; }
    }
    public class CancelListDataUri
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

        public DateType DateType { get; set; } = DateType.Cancellation;
        public string DateTypeText { get; set; }
        public SortCancel Sort { get; set; } = SortCancel.Customer;
        public string SortText { get; set; }
        //Change item search
        public ReservationClassComponentData YoyakuFrom { get; set; }
        public ReservationClassComponentData YoyakuTo { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Today;
        public DateTime EndDate { get; set; } = DateTime.Today;
        public CompanyData Company { get; set; }
        public TPM_CodeKbData CancelBookingType { get; set; }
        public SelectedOption<ConfirmAction> CancelCharge { get; set; }
        public SelectedOption<BreakReportPage> BreakPage { get; set; }
        public LoadSaleBranch BranchStart { get; set; }
        public LoadSaleBranch BranchEnd { get; set; }
        // 得意先

        // New
        public CustomerComponentGyosyaData GyosyaTokuiSakiFrom { get; set; }
        public CustomerComponentGyosyaData GyosyaTokuiSakiTo { get; set; }
        public CustomerComponentTokiskData TokiskTokuiSakiFrom { get; set; }
        public CustomerComponentTokiskData TokiskTokuiSakiTo { get; set; }
        public CustomerComponentTokiStData TokiStTokuiSakiFrom { get; set; }
        public CustomerComponentTokiStData TokiStTokuiSakiTo { get; set; }

        // 仕入先

        // New
        public CustomerComponentGyosyaData GyosyaShiireSakiFrom { get; set; }
        public CustomerComponentGyosyaData GyosyaShiireSakiTo { get; set; }
        public CustomerComponentTokiskData TokiskShiireSakiFrom { get; set; }
        public CustomerComponentTokiskData TokiskShiireSakiTo { get; set; }
        public CustomerComponentTokiStData TokiStShiireSakiFrom { get; set; }
        public CustomerComponentTokiStData TokiStShiireSakiTo { get; set; }
        public LoadStaff StaffStart { get; set; }
        public LoadStaff StaffEnd { get; set; }
        public LoadStaff CancelStaffStart { get; set; }
        public LoadStaff CancelStaffEnd { get; set; }
        public OutputReportType ExportType { get; set; } = OutputReportType.Preview;
        public PaperSize PaperSize { get; set; }
        public CsvConfigOption CsvConfigOption { get; set; } = new CsvConfigOption();

    }
}
