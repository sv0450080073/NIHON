using HassyaAllrightCloud.Commons.Constants;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class TaxTypeList
    {
        public int IdValue { get; set; }
        public string StringValue { get; set; }
    }

    public class TaxTypeListData
    {
        public static List<TaxTypeList> taxTypeList = new List<TaxTypeList>
        {
            Constants.ForeignTax,
            Constants.InTax,
            Constants.NoTax
        };

    }
}
