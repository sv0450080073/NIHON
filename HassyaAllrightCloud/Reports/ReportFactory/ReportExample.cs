using DevExpress.XtraReports.Native.Parameters;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Reports.ReportFactory;
using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;

namespace HassyaAllrightCloud.Reports.ReportFactory
{
    public class ReportExample : ReportBase
    {
        public ReportExample(string reportName, string displayname)
        {
            ReportName = reportName;
            DisplayName = displayname;
        }

        public override Func<Dictionary<string, string>, XtraReport> CreateByParam
        {
            get
            {
                return (parameters) =>
                {
                    var report = new Reports.ReportTemplate.Example.TestTemplate();
                    report.Parameters["MaxUkeCd"].Value = parameters["MaxUkeCd"];
                    report.Parameters["MinUkeCd"].Value = parameters["MinUkeCd"];
                    return report;
                };
            }
        }
    }
    public class ReportTestCreator : ReportCreator
    {
        private string _reportName;
        private string _displayName;
        public ReportTestCreator(string reportName, string displayName)
        {
            _reportName = reportName;
            _displayName = displayName;
        }
        public override ReportBase GetReport()
        {
            return new ReportExample(_reportName, _displayName);
        }
    }
}
