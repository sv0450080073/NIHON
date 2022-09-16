using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class CsvConfigOption
    {
        public string FilePath { get; set; }
        public SelectedOption<CSV_Group> GroupSymbol { get; set; }
        public SelectedOption<CSV_Header> Header { get; set; }
        public SelectedOption<CSV_Delimiter> Delimiter { get; set; }

        public string DelimiterSymbol { get; set; }
    }
}
