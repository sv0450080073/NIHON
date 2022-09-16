using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class VehicleStatisticsSurveyDS
    {
        public List<VehicleStatisticsSurveyPDF> _data { get; set; }
        public VehicleStatisticsSurveyDS(List<VehicleStatisticsSurveyPDF> data)
        {
            _data = data;
        }
    }
}
