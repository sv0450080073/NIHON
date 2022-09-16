namespace HassyaAllrightCloud.Domain.Dto.SubContractorStatus
{
    using HassyaAllrightCloud.Commons.Extensions;
    using System.Collections.Generic;
    using System.Linq;

    public class SubContractorStatusSearchPaged
    {
        public int TotalRecord { get; set; }
        public List<SubContractorStatusSearchResultData> PageData { get; set; } = new List<SubContractorStatusSearchResultData>();
        public SubContractorStatusSummaryData Summary { get; set; }
        public SubContractorStatusSummaryData CurrentPageSummary
        {
            get
            {
                if (PageData is null)
                    return new SubContractorStatusSummaryData();

                var summaryPage = new SubContractorStatusSummaryData();

                foreach (var item in PageData.DistinctBy(_ => new {_.UkeCd, _.UnkRen }))
                {
                    summaryPage._totalSyaRyoUnc += item._sumSyaRyoUnc;
                    summaryPage._totalZeiRui += item._sumZeiRui;
                    summaryPage._totalTesuRyoG += item._sumTesuRyoG;
                    
                    summaryPage._totalGuideFee += (long)item._sumGuideFee;
                    summaryPage._totalGuideTax += (long)item._sumGuideTax;
                    summaryPage._totalUnitGuiderFee += (long)item._sumUnitGuiderFee;
                    
                    summaryPage._totalIncidentalFee += item._incidentalFee;
                    summaryPage._totalIncidentalTax += item._incidentalTax;
                    summaryPage._totalIncidentalCharge += item._incidentalCharge;
                }

                foreach (var item in PageData)
                {
                    foreach (var fee in item.TaxFeeInfos)
                    {
                        summaryPage._totalYoushaUnc += fee._youshaUnc;
                        summaryPage._totalYoushaSyo += fee._youshaSyo;
                        summaryPage._totalYoushaTes += fee._youshaTes;
                    }

                    summaryPage._totalYouFutTumGuiKin += (long)item._youFutTumGuiKin;
                    summaryPage._totalYouFutTumGuiTax += (long)item._youFutTumGuiTax;
                    summaryPage._totalYouFutTumGuiTes += (long)item._youFutTumGuiTes;

                    summaryPage._totalYouFutTumKin += item._youFutTumKin;
                    summaryPage._totalYouFutTumTax += item._youFutTumTax;
                    summaryPage._totalYouFutTumTes += item._youFutTumTes;
                }
                return summaryPage;
            }
        }
    }
}
