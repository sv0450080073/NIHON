using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class StatusConfirmationPagedSearch
    {
        public int TotalItems { get; set; }
        public List<StatusConfirmSearchResultData> DataList { get; set; }
        public PageSummaryData TotalSummary { get; set; }

        public StatusConfirmationPagedSearch()
        {
            DataList = new List<StatusConfirmSearchResultData>();
            TotalSummary = new PageSummaryData();
        }
    }
}