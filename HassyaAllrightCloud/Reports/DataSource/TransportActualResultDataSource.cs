using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Reports.DataSource
{
    public class TransportActualResultDataSource
    {
        public List<TransportActualResultReportData> DataSource { get; set; }
        public TransportActualResultDataSource(List<TransportActualResultReportData> dataSource)
        {
            DataSource = dataSource;
        }
    }
}
