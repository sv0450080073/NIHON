using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class BusinessPlanReceivableReportDS
    {
        public List<BusinessPlanReceivableListReportDataModel> _data { get; set; }
        public BusinessPlanReceivableReportDS(List<BusinessPlanReceivableListReportDataModel> data)
        {
            _data = data;
        }
    }
}
