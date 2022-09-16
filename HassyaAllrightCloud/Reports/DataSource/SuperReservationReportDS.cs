using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class SuperReservationReportDS
    {
        public List<SuperMenuReservationReportPDF> _data { get; set; }
        public SuperReservationReportDS(List<SuperMenuReservationReportPDF> data)
        {
            _data = data;
        }
    }
}
