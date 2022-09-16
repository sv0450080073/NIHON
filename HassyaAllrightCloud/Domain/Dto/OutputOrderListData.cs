using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Domain.Dto
{
    public class OutputOrderData
    {
        public int IdValue { get; set; }
        public string StringValue { get; set; }
    }

    public class OutputOrderListData
    {
        public static List<OutputOrderData> OutputOrderlst = new List<OutputOrderData>
        {
            new OutputOrderData { IdValue = 1, StringValue = "出庫・車両コード順", },
            new OutputOrderData { IdValue = 2, StringValue = "出庫・梯団順"},
            new OutputOrderData { IdValue = 3, StringValue = "車両コード順"},
            new OutputOrderData { IdValue = 4, StringValue = "車両点呼順"},
            new OutputOrderData { IdValue = 5, StringValue = "出庫・乗務員コード順"}
        };
    }
}
