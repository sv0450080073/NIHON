using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class BusCoordinationReportReportDS
    {    
            public List<BusCoordinationReportPDF> _data { get; set; }
            public BusCoordinationReportReportDS(List<BusCoordinationReportPDF> data)
            {
                _data = data;
            }
      
    }
}
