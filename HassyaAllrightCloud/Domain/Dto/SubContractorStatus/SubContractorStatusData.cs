using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class SubContractorStatusData
    {
        public DateType DateType { get; set; }
        public string DateTypeText { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Today;
        public DateTime EndDate { get; set; } = DateTime.Today;
        public LoadCustomerList CustomerStart { get; set; }
        public LoadCustomerList CustomerEnd { get; set; }
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
        public List<ReservationData> BookingTypes { get; set; }
        public ReservationClassComponentData RegistrationTypeFrom { get; set; }
        public ReservationClassComponentData RegistrationTypeTo { get; set; }
        public List<CompanyData> Companies { get; set; }
        public List<int> CompanyIds { get; set; }
        public LoadSaleBranch BranchStart { get; set; }
        public LoadSaleBranch BranchEnd { get; set; }
        public LoadStaff StaffStart { get; set; }
        public LoadStaff StaffEnd { get; set; }
        public ViewMode FormViewSize { get; set; } = ViewMode.Large;
        public SelectedOption<OwnCompanyType> OwnCompanyType { get; set; }
        public SelectedOption<ContractorOutputOrder> OutputOrder { get; set; }
        public SelectedOption<GroupDivision> Group { get; set; }
        public SelectedOption<PageBreak> PageBreak { get; set; }
        public SelectedOption<BreakReportPage> BreakReportPage { get; set; }
        public OutputReportType ExportType { get; set; }
        public PaperSize PaperSize { get; set; }
        public CustomerComponentGyosyaData SelectedGyosyaFrom { get; set; }
        public CustomerComponentTokiskData SelectedTokiskFrom { get; set; }
        public CustomerComponentTokiStData SelectedTokiStFrom { get; set; }
        public CustomerComponentGyosyaData SelectedGyosyaTo { get; set; }
        public CustomerComponentTokiskData SelectedTokiskTo { get; set; }
        public CustomerComponentTokiStData SelectedTokiStTo { get; set; }
        public int SelectedReservationFromSeq { get; set; }
        public int SelectedReservationToSeq { get; set; }
        public CsvConfigOption CsvConfigOption { get; set; } = new CsvConfigOption();

        /// <summary>
        /// Convert this object to a <see cref="Dictionary{string, string}"> 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> ToDictionary()
        {
            return new Dictionary<string, string>
            {
                [nameof(DateType)] = DateType.ToString(),
                [nameof(StartDate)] = StartDate.ToString("yyyyMMdd"),
                [nameof(EndDate)] = EndDate.ToString("yyyyMMdd"),
                // N.T.Hieu Add str 2021/06/07
                [nameof(SelectedGyosyaFrom)] = SelectedGyosyaFrom?.GyosyaCdSeq.ToString() ?? "-1",
                [nameof(SelectedTokiskFrom)] = SelectedTokiskFrom?.TokuiSeq.ToString() ?? "-1",
                [nameof(SelectedTokiStFrom)] = SelectedTokiStFrom?.SitenCdSeq.ToString() ?? "-1",
                [nameof(SelectedGyosyaTo)] = SelectedGyosyaTo?.GyosyaCdSeq.ToString() ?? "-1",
                [nameof(SelectedTokiskTo)] = SelectedTokiskTo?.TokuiSeq.ToString() ?? "-1",
                [nameof(SelectedTokiStTo)] = SelectedTokiStTo?.SitenCdSeq.ToString() ?? "-1",
                [nameof(RegistrationTypeFrom)] = RegistrationTypeFrom?.YoyaKbnSeq.ToString() ?? "-1",
                [nameof(RegistrationTypeTo)] = RegistrationTypeTo?.YoyaKbnSeq.ToString() ?? "-1",
                [nameof(UkeCdFrom)] = UkeCdFrom,
                [nameof(UkeCdTo)] = UkeCdTo,
                [nameof(Companies)] = string.Join('-', Companies.Select(_ => _.CompanyCdSeq)),
                [nameof(BranchStart)] = BranchStart?.EigyoCdSeq.ToString() ?? string.Empty,
                [nameof(BranchEnd)] = BranchEnd?.EigyoCdSeq.ToString() ?? string.Empty,
                [nameof(StaffStart)] = StaffStart?.SyainCdSeq.ToString() ?? string.Empty,
                [nameof(StaffEnd)] = StaffEnd?.SyainCdSeq.ToString() ?? string.Empty,
                [nameof(FormViewSize)] = FormViewSize.ToString(),
                [nameof(OwnCompanyType)] = OwnCompanyType.Option.ToString(),
                [nameof(OutputOrder)] = OutputOrder.Option.ToString(),
                [nameof(Group)] = Group.Option.ToString(),
                [nameof(PageBreak)] = PageBreak.Option.ToString(),
                [nameof(ExportType)] = ExportType.ToString(),
                [nameof(PaperSize)] = PaperSize.ToString(),
                [nameof(CsvConfigOption.Header)] = CsvConfigOption.Header.Option.ToString(),
                [nameof(CsvConfigOption.GroupSymbol)] = CsvConfigOption.GroupSymbol.Option.ToString(),
                [nameof(CsvConfigOption.Delimiter)] = CsvConfigOption.Delimiter.Option.ToString(),
                [nameof(CsvConfigOption.DelimiterSymbol)] = CsvConfigOption.DelimiterSymbol ?? string.Empty,
            };
        }

        /// <summary>
        /// Apply value data from <see cref="Dictionary{string, string}"/> into this object
        /// </summary>
        /// <param name="dic">Data source</param>
        public void ApplyDictionary(ref SubContractorStatusData data, DefaultCustomerData customerFrom, DefaultCustomerData customerTo, int regisFrom, int regisTo, Dictionary<string, string> dic)
        {
            if (Enum.TryParse<DateType>(dic[nameof(DateType)], out DateType dt))
            {
                DateType = dt;
            }
            if (DateTime.TryParseExact(dic[nameof(StartDate)], "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime dateTime))
            {
                StartDate = dateTime;
            }
            if (DateTime.TryParseExact(dic[nameof(EndDate)], "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out dateTime))
            {
                EndDate = dateTime;
            }

            if (int.TryParse(dic[nameof(SelectedGyosyaFrom)], out int val))
                customerFrom.GyosyaCdSeq = val == 0 ? -1 : val;
            if (int.TryParse(dic[nameof(SelectedTokiskFrom)], out val))
                customerFrom.TokiskCdSeq = val == 0 ? -1 : val;
            if (int.TryParse(dic[nameof(SelectedTokiStFrom)], out val))
                customerFrom.TokiStCdSeq = val == 0 ? -1 : val;
            if (int.TryParse(dic[nameof(SelectedGyosyaTo)], out val))
                customerTo.GyosyaCdSeq = val == 0 ? -1 : val;
            if (int.TryParse(dic[nameof(SelectedTokiskTo)], out val))
                customerTo.TokiskCdSeq = val == 0 ? -1 : val;
            if (int.TryParse(dic[nameof(SelectedTokiStTo)], out val))
                customerTo.TokiStCdSeq = val == 0 ? -1 : val;

            UkeCdFrom = dic[nameof(UkeCdFrom)];
            UkeCdTo = dic[nameof(UkeCdTo)];

            if (int.TryParse(dic[nameof(RegistrationTypeFrom)], out val))
                regisFrom = val == 0 ? -1 : val;
            if (int.TryParse(dic[nameof(RegistrationTypeTo)], out val))
                regisTo = val == 0 ? -1 : val;

            var cmpIds = string.IsNullOrEmpty(dic[nameof(Companies)]) ? null : dic[nameof(Companies)].Split('-').Select(_ => int.Parse(_));
            Companies = cmpIds?.Select(cmp => new CompanyData { CompanyCdSeq = cmp, IsSelectedAll = cmp == 0 }).ToList() ?? new List<CompanyData>();
            if (int.TryParse(dic[nameof(BranchStart)], out int outValue))
            {
                BranchStart = new LoadSaleBranch() { EigyoCdSeq = outValue };
            }
            if (int.TryParse(dic[nameof(BranchEnd)], out outValue))
            {
                BranchEnd = new LoadSaleBranch() { EigyoCdSeq = outValue };
            }
            if (int.TryParse(dic[nameof(StaffStart)], out outValue))
            {
                StaffStart = new LoadStaff { SyainCdSeq = outValue };
            }
            if (int.TryParse(dic[nameof(StaffEnd)], out outValue))
            {
                StaffEnd = new LoadStaff { SyainCdSeq = outValue };
            }

            if (Enum.TryParse(dic[nameof(FormViewSize)], out ViewMode vm))
            {
                FormViewSize = vm;
            }
            OwnCompanyType = new SelectedOption<OwnCompanyType>() { Option = Enum.Parse<OwnCompanyType>(dic[nameof(OwnCompanyType)]) };
            OutputOrder = new SelectedOption<ContractorOutputOrder>() { Option = Enum.Parse<ContractorOutputOrder>(dic[nameof(OutputOrder)]) };
            Group = new SelectedOption<GroupDivision>() { Option = Enum.Parse<GroupDivision>(dic[nameof(Group)]) };
            PageBreak = new SelectedOption<PageBreak>() { Option = Enum.Parse<PageBreak>(dic[nameof(PageBreak)]) };
            if (Enum.TryParse(dic[nameof(ExportType)], out OutputReportType ort))
            {
                ExportType = ort;
            }
            if (Enum.TryParse(dic[nameof(PaperSize)], out PaperSize ps))
            {
                PaperSize = ps;
            }

            CsvConfigOption.Header = new SelectedOption<CSV_Header>() { Option = Enum.Parse<CSV_Header>(dic[nameof(CsvConfigOption.Header)]) };
            CsvConfigOption.GroupSymbol = new SelectedOption<CSV_Group>() { Option = Enum.Parse<CSV_Group>(dic[nameof(CsvConfigOption.GroupSymbol)]) };
            CsvConfigOption.Delimiter = new SelectedOption<CSV_Delimiter>() { Option = Enum.Parse<CSV_Delimiter>(dic[nameof(CsvConfigOption.Delimiter)]) };
            CsvConfigOption.DelimiterSymbol = dic[nameof(CsvConfigOption.DelimiterSymbol)];
        }
    }
}
