using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.WebDocumentViewer;
using HassyaAllrightCloud.Domain.Dto.BillPrint;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.Reports.DataSource;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Reports.ReportFactory
{
    public class CustomWebDocumentViewerReportResolver : IWebDocumentViewerReportResolver
    {
        private IServiceProvider _serviceProvider { get; set; }
        private string nameSpace = "HassyaAllrightCloud.IService.";
        public CustomWebDocumentViewerReportResolver() { }
        public CustomWebDocumentViewerReportResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public XtraReport Resolve(string reportEntry)
        {
            if (reportEntry.Split('?')[0].EndsWith("Service"))
            {
                var type = Type.GetType(nameSpace + reportEntry.Split('?')[0]);
                var service = _serviceProvider.GetRequiredService(type) as IReportService;
                var report = service.PreviewReport(reportEntry.Split('?')[1]).Result;
                return report;
            }
            else
            {
                var reportSource = _serviceProvider.GetRequiredService<IKashikiryReportSource>();
                return reportSource.GetReport(reportEntry);
            }
        }
    }
}
