using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Commons.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Reports.ReportFactory
{
    public class ReportHikiukesho : ReportBase
    {
        public ReportHikiukesho(string reportName, string displayname)
        {
            ReportName = reportName;
            DisplayName = displayname;
        }
        public override Func<string, XtraReport> CreateByUrl
        {
            get
            {
                return (url) =>
                {
                    var encryptedParameters = String.Join('?', url.Split('?').Skip(1)).Split('=')[1];
                    Dictionary<string, string> parameters = EncryptHelper.DecryptFromUrl<Dictionary<string, string>>(encryptedParameters);
                    var reportTemplate = parameters.GetValueOrDefault("ReportTemplate");
                    var report = (XtraReport)Activator.CreateInstance(Type.GetType(reportTemplate));
                    report.Parameters["TenantCdSeq"].Value = parameters.GetValueOrDefault("TenantCdSeq");
                    report.Parameters["TenantCdSeq"].Visible = false;

                    report.Parameters["UkeNo"].Value = parameters.GetValueOrDefault("UkeNumberFull");
                    report.Parameters["UkeNo"].Visible = false;

                    report.Parameters["UnkRen"].Value = parameters.GetValueOrDefault("UnkRen");
                    report.Parameters["UnkRen"].Visible = false;

                    report.Parameters["OutputUnit"].Value = parameters.GetValueOrDefault("OutputUnit");
                    report.Parameters["OutputUnit"].Visible = false;

                    report.Parameters["StartDispatchDate"].Value = parameters.GetValueOrDefault("StartDispatchDate");
                    report.Parameters["StartDispatchDate"].Visible = false;
                    report.Parameters["EndDispatchDate"].Value = parameters.GetValueOrDefault("EndDispatchDate");
                    report.Parameters["EndDispatchDate"].Visible = false;

                    report.Parameters["StartArrivalDate"].Value = parameters.GetValueOrDefault("StartArrivalDate");
                    report.Parameters["StartArrivalDate"].Visible = false;
                    report.Parameters["EndArrivalDate"].Value = parameters.GetValueOrDefault("EndArrivalDate");
                    report.Parameters["EndArrivalDate"].Visible = false;

                    report.Parameters["StartReservationDate"].Value = parameters.GetValueOrDefault("StartReservationDate");
                    report.Parameters["StartReservationDate"].Visible = false;
                    report.Parameters["EndReservationDate"].Value = parameters.GetValueOrDefault("EndReservationDate");
                    report.Parameters["EndReservationDate"].Visible = false;

                    report.Parameters["GyosyaCd"].Value = parameters.GetValueOrDefault("GyosyaCd");
                    report.Parameters["GyosyaCd"].Visible = false;

                    report.Parameters["TokuiCd"].Value = parameters.GetValueOrDefault("TokuiCd");
                    report.Parameters["TokuiCd"].Visible = false;

                    report.Parameters["SitenCd"].Value = parameters.GetValueOrDefault("SitenCd");
                    report.Parameters["SitenCd"].Visible = false;

                    report.Parameters["UkeEigCd"].Value = parameters.GetValueOrDefault("UkeEigCd");
                    report.Parameters["UkeEigCd"].Visible = false;

                    report.Parameters["EigSyainCd"].Value = parameters.GetValueOrDefault("EigSyainCd");
                    report.Parameters["EigSyainCd"].Visible = false;

                    report.Parameters["InpSyainCd"].Value = parameters.GetValueOrDefault("InpSyainCd");
                    report.Parameters["InpSyainCd"].Visible = false;

                    report.Parameters["YoyaKbnList"].Value = parameters.GetValueOrDefault("YoyaKbnList");
                    report.Parameters["YoyaKbnList"].Visible = false;

                    report.Parameters["YearlyContract"].Value = parameters.GetValueOrDefault("YearlyContract");
                    report.Parameters["YearlyContract"].Visible = false;

                    report.Parameters["OutputSelection"].Value = parameters.GetValueOrDefault("OutputSelection");
                    report.Parameters["OutputSelection"].Visible = false;
                    return report;
                };
            }
        }

        public override Func<Dictionary<string, string>, XtraReport> CreateByParam => throw new NotImplementedException();
    }

    public class ReportHikiukeshoCreator : ReportCreator
    {
        private string _reportName;
        private string _displayName;
        public ReportHikiukeshoCreator(string reportName, string displayName)
        {
            _reportName = reportName;
            _displayName = displayName;
        }

        public override ReportBase GetReport()
        {
            return new ReportHikiukesho(_reportName, _displayName);
        }
    }
}
