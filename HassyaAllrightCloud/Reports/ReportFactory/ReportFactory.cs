using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HassyaAllrightCloud.Reports.ReportFactory
{
    public interface IKashikiryReportSource
    {
        XtraReport GetReport(string url);

        XtraReport GetReport(string reportName, Dictionary<string, string> parameters);

        Dictionary<string, string> GetReportList();
    }

    public abstract class ReportBase
    {
        public string ReportName { get; set; }
        public string DisplayName { get; set; }

        public virtual Func<string, XtraReport> CreateByUrl
        {
            get
            {
                return (url) =>
                {
                    var query = String.Join('?', url.Split('?').Skip(1));
                    var parameters = HttpUtility.ParseQueryString(query);
                    var dictionary = parameters.AllKeys.ToDictionary(key => key, key => parameters[key]);
                    return CreateByParam(dictionary);
                };
            }
        }

        public abstract Func<Dictionary<string, string>, XtraReport> CreateByParam { get; }
    }

    public abstract class ReportCreator
    {
        public abstract ReportBase GetReport();
    }

    public class KashikiriReportSourceFactory : IKashikiryReportSource
    {
        private readonly List<ReportBase> predefinedReports = new List<ReportBase>
        {
            new ReportFromFileCreator(nameof(ReportFromFile), "Template from file").GetReport(),
            new ReportTestCreator(nameof(ReportExample), "Test Template").GetReport(),
            new ReportHikiukeshoCreator("HikiukeshoReport", "Hikiukesho Report Template").GetReport(),
            new ReportTenkokirokuCreator(nameof(ReportTenkokiroku),"Report Tenkokiroku").GetReport(),
            new ReportUnkoushijishoBaseCreator(nameof(ReportUnkoushijishoBase),"Report Unkoushijisho").GetReport(),
            new ReportUnkoushijishoCreator(nameof(ReportUnkoushijisho),"Report Unkoushijisho").GetReport(),
            new BusReportCreator(nameof(BusReport),"Report BusReport").GetReport(),
            new ReportJomukirokuboCreator(nameof(ReportJomukirokubo),"Report Jomukirokubo").GetReport(),
        };

        private static object ReporBasetUnkoushijisho()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> GetReportList()
        {
            return predefinedReports.ToDictionary(i => i.ReportName, i => i.DisplayName);
        }

        public XtraReport GetReport(string url)
        {
            var reportName = url.Split('?')[0];
            return predefinedReports.FirstOrDefault(x => reportName.ToLower().Contains(x.ReportName.ToLower()))?.CreateByUrl(url);
        }

        public XtraReport GetReport(string reportName, Dictionary<string, string> parameters)
        {
            return predefinedReports.FirstOrDefault(x => reportName.ToLower().Contains(x.ReportName.ToLower()))?.CreateByParam(parameters);
        }
    }
}