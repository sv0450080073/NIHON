using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class AttendanceConfirmReportDS
    {
        public List<AttendanceConfirmReportPDF> _data { get; set; }
        public AttendanceConfirmReportDS(List<AttendanceConfirmReportPDF> data)
        {
            _data = data;
        }
    }
}
