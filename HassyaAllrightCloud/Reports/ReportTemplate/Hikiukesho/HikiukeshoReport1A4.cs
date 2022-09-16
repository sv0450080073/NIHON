using System;
using DevExpress.XtraReports.UI;

namespace HassyaAllrightCloud.Reports
{
    public partial class HikiukeshoReport1A4
    {
        public HikiukeshoReport1A4()
        {
            InitializeComponent();
        }

        private void HikiukeshoReport_AfterPrint(object sender, EventArgs e)
        {

            PrintingSystem.Document.Name = "HikiukeshoReport" + DateTime.Now.ToString("yyyyMMddHHmm");
        }
    }
}
