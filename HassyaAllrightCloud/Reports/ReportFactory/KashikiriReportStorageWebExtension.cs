using System;
using System.Collections.Generic;
using System.IO;
using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Hosting;

namespace HassyaAllrightCloud.Reports.ReportFactory
{
    public class KashikiriReportStorageWebExtension : DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension
    {
        protected IWebHostEnvironment Environment { get; }
        protected IKashikiryReportSource PredefinedReports { get; }
        readonly string ReportDirectory;

        public KashikiriReportStorageWebExtension(IWebHostEnvironment env, IKashikiryReportSource  reportFactory)
        {
            Environment = env;
            PredefinedReports = reportFactory;
            ReportDirectory = Path.Combine(env.ContentRootPath, "Reports");
            if (!Directory.Exists(ReportDirectory))
            {
                Directory.CreateDirectory(ReportDirectory);
            }
        }

        /// <summary>
        /// Check if report can be save with specific url in report designer.
        /// Determines whether or not it is possible to store a report by a given URL. 
        /// For instance, make the CanSetData method return false for reports that should be read-only in your storage. 
        /// This method is called only for valid URLs (i.e., if the IsValidUrl method returned true) before the SetData method is called.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>false can not edit report</returns>
        public override bool CanSetData(string url)
        {
            return false;
        }

        /// <summary>
        /// Determines whether or not the URL passed to the current Report Storage is valid. 
        /// For instance, implement your own logic to prohibit URLs that contain white spaces or some other special characters. 
        /// This method is called before the CanSetData and GetData methods.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public override bool IsValidUrl(string url)
        {
            var reportName = url.Split('?')[0];
            if (PredefinedReports.GetReportList().ContainsKey(reportName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns report layout data stored in a Report Storage using the specified URL. 
        /// This method is called only for valid URLs after the IsValidUrl method is called.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public override byte[] GetData(string url)
        {
            url = $"{url}&folder={ReportDirectory}";
            XtraReport report = PredefinedReports.GetReport(url);
            if (report == null)
            {
                throw new Exception("Report was not found.");
            }
            
            using (var stream = new MemoryStream())
            {
                report.SaveLayoutToXml(stream);
                report.Dispose();
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Returns a dictionary of the existing report URLs and display names. 
        /// This method is called when running the Report Designer, 
        /// before the Open Report and Save Report dialogs are shown and after a new report is saved to a storage.
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, string> GetUrls()
        {
            var predefinedList = PredefinedReports.GetReportList();
            return predefinedList;
        }


        /// <summary>
        /// Used by report design component
        /// Stores the specified report to a Report Storage using the specified URL. 
        /// This method is called only after the IsValidUrl and CanSetData methods are called.
        /// </summary>
        /// <param name="report"></param>
        /// <param name="url"></param>
        public override void SetData(XtraReport report, string url)
        {
            using (var stream = new MemoryStream())
            {
                report.SaveLayoutToXml(stream);
            }
        }

        /// <summary>
        /// Used by report design component
        /// Stores the specified report using a new URL. 
        /// The IsValidUrl and CanSetData methods are never called before this method. 
        /// You can validate and correct the specified URL directly in the SetNewData method implementation 
        /// and return the resulting URL used to save a report in your storage.
        /// </summary>
        /// <param name="report"></param>
        /// <param name="defaultUrl"></param>
        /// <returns></returns>
        public override string SetNewData(XtraReport report, string defaultUrl)
        {
            using (var stream = new MemoryStream())
            {
                report.SaveLayoutToXml(stream);
            }
            return defaultUrl;
        }
    }
}
