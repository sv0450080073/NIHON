using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IReportService
    {
        Task<XtraReport> PreviewReport(string queryParams);
    }
}
