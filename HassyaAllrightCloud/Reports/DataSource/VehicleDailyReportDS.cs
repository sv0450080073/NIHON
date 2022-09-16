using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.XtraPrinting.BarCode;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class VehicleDailyReportDS
    {
        public List<VehicleDailyReportPDF> _data { get; set; }
        public VehicleDailyReportDS(List<VehicleDailyReportPDF> data)
        {
            _data = data;
        }
    }
}
