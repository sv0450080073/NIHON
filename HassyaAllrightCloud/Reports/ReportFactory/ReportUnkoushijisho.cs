using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Reports.ReportFactory
{
    public class ReportUnkoushijisho:ReportBase
    {
        public ReportUnkoushijisho(string reportName, string displayname)
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
                    string datedefault = (new DateTime()).ToString("yyyyMMdd");
                    var report = new UnkoushijishoReport1A4();
                    report.Parameters["TenantCdSeq"].Value = parameters["TenantCdSeq"] != null ? parameters["TenantCdSeq"] : "";
                    report.Parameters["TenantCdSeq"].Visible = false;
                    report.Parameters["SyuKoYmd"].Value = parameters["SyuKoYmd"] != datedefault ? parameters["SyuKoYmd"] : "";
                    report.Parameters["SyuKoYmd"].Visible = false;
                    report.Parameters["UkeCdFrom"].Value = parameters["UkeCdFrom"] != null ? parameters["UkeCdFrom"] : "";
                    report.Parameters["UkeCdFrom"].Visible = false;
                    report.Parameters["UkeCdTo"].Value = parameters["UkeCdTo"] != null ? parameters["UkeCdTo"] : "";
                    report.Parameters["UkeCdTo"].Visible = false;
                    report.Parameters["YoyakuFrom"].Value = parameters["YoyakuFrom"] != null ? parameters["YoyakuFrom"] : "0";
                    report.Parameters["YoyakuFrom"].Visible = false;
                    report.Parameters["YoyakuTo"].Value = parameters["YoyakuTo"] != null ? parameters["YoyakuTo"] : "0";
                    report.Parameters["YoyakuTo"].Visible = false;
                    report.Parameters["SyuEigCdSeq"].Value = parameters["SyuEigCdSeq"] != null ? parameters["SyuEigCdSeq"] : "";
                    report.Parameters["SyuEigCdSeq"].Visible = false;
                    report.Parameters["TeiDanNo"].Value = parameters["TeiDanNo"] != null ? parameters["TeiDanNo"] : "";
                    report.Parameters["TeiDanNo"].Visible = false;
                    report.Parameters["UnkRen"].Value = parameters["UnkRen"] != null ? parameters["UnkRen"] : "";
                    report.Parameters["UnkRen"].Visible = false;
                    report.Parameters["BunkRen"].Value = parameters["BunkRen"] != null ? parameters["BunkRen"] : "";
                    report.Parameters["BunkRen"].Visible = false;
                    report.Parameters["SortOrder"].Value = parameters["SortOrder"] != null ? parameters["SortOrder"] : "";
                    report.Parameters["SortOrder"].Visible = false;
                    report.Parameters["UkenoList"].Value = parameters["UkenoList"] != null ? parameters["UkenoList"] : "";
                    report.Parameters["UkenoList"].Visible = false;
                    report.Parameters["FormOutput"].Value = parameters["FormOutput"] != null ? parameters["FormOutput"] : "0";
                    report.Parameters["FormOutput"].Visible = false;
                    report.DisplayName = "Unkoushijisho";
                    return report;
                };
            }
        }
    }
    public class ReportUnkoushijishoCreator : ReportCreator
    {
        private string _reportName;
        private string _displayName;
        public ReportUnkoushijishoCreator(string reportName, string displayName)
        {
            _reportName = reportName;
            _displayName = displayName;
        }

        public override ReportBase GetReport()
        {
            return new ReportUnkoushijisho(_reportName, _displayName);
        }
    }
}
