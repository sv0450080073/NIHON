using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Reports.ReportFactory
{
   public class BusReport : ReportBase
    {
        public BusReport(string reportName, string displayname)
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
                    var reportTemplate = parameters.GetValueOrDefault("ReportTemplate");
                    var report = (XtraReport)Activator.CreateInstance(Type.GetType(reportTemplate));
                    report.Parameters["DateBooking"].Value = parameters["DateBooking"] != null ? parameters["DateBooking"] : "";
                    report.Parameters["DateBooking"].Visible = false;
                    report.Parameters["ListCompany"].Value = parameters["ListCompany"] != null ? parameters["ListCompany"] : "";
                    report.Parameters["ListCompany"].Visible = false;
                    report.Parameters["BranchFrom"].Value = parameters["BranchFrom"] != null ? parameters["BranchFrom"] : "";
                    report.Parameters["BranchFrom"].Visible = false;
                    report.Parameters["BranchTo"].Value = parameters["BranchTo"] != null ? parameters["BranchTo"] : "";
                    report.Parameters["BranchTo"].Visible = false;
                    report.Parameters["BookingTypeList"].Value = parameters["BookingTypeList"] != null ? parameters["BookingTypeList"] : "";
                    report.Parameters["BookingTypeList"].Visible = false;
                    report.Parameters["MihaisyaKbn"].Value = parameters["MihaisyaKbn"] != null ? parameters["MihaisyaKbn"] : "";
                    report.Parameters["MihaisyaKbn"].Visible = false;
                    /* report.Parameters["BookingTypeTo"].Visible = false;
                    report.Parameters["MihaisyaKbn"].Value = parameters["MihaisyaKbn"] != null ? parameters["MihaisyaKbn"] : "";*/
                    report.Parameters["Order"].Value = parameters["Order"] != null ? parameters["Order"] : "";
                    report.Parameters["Order"].Visible = false;
                    report.Parameters["TenantCdSeq"].Value = parameters["TenantCdSeq"] != null ? parameters["TenantCdSeq"] : "";
                    report.Parameters["TenantCdSeq"].Visible = false;                    
                    report.Parameters["SyainNm"].Value = parameters["SyainNm"] != null ? parameters["SyainNm"] : "";
                    report.Parameters["SyainNm"].Visible = false;
                    report.Parameters["DateTimeHeader"].Value = parameters["DateTimeHeader"] != null ? parameters["DateTimeHeader"] : "";
                    report.Parameters["DateTimeHeader"].Visible = false;
                    report.Parameters["DateTimeFooter"].Value = parameters["DateTimeFooter"] != null ? parameters["DateTimeFooter"] : "";
                    report.Parameters["DateTimeFooter"].Visible = false;
                    report.Parameters["SyainCd"].Value = parameters["SyainCd"] != null ? parameters["SyainCd"] : "";
                    report.Parameters["SyainCd"].Visible = false;
                    report.DisplayName = "Haisyahyo";
                    return report;
                };
            }
        }
    }
    public class BusReportCreator : ReportCreator
    {
        private string _reportName;
        private string _displayName;
        public BusReportCreator(string reportName, string displayName)
        {
            _reportName = reportName;
            _displayName = displayName;
        }
        public override ReportBase GetReport()
        {
            return new BusReport(_reportName, _displayName);
        }
    }
}
