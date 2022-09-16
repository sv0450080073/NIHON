using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class SuperVehicleReportDS
    {
        public List<SuperMenuVehicleReportPDF> _data { get; set; }
        public SuperVehicleReportDS(List<SuperMenuVehicleReportPDF> data)
        {
            _data = data;
        }
    }
}
