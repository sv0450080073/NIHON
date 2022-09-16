using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Reports.ReportFactory
{
    public class ReportFromFile : ReportBase
    {
        public ReportFromFile(string reportName, string displayname)
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
                    var folder = parameters["folder"];
                    var fileName = parameters["file"];
                    var filepath = Path.Combine(folder, fileName);
                    if (!File.Exists(filepath))
                    {
                        return null;
                    }
                    var report = XtraReport.FromFile(filepath);
                    report.Parameters["MaxUkeCd"].Value = parameters["MaxUkeCd"];
                    report.Parameters["MinUkeCd"].Value = parameters["MinUkeCd"];
                    return report;
                };
            }
        }
    }

    public class ReportFromFileCreator : ReportCreator
    {
        private string _reportName;
        private string _displayName;
        public ReportFromFileCreator(string reportName, string displayName)
        {
            _reportName = reportName;
            _displayName = displayName;
        }

        public override ReportBase GetReport()
        {
            return new ReportFromFile(_reportName, _displayName);
        }
    }
}
