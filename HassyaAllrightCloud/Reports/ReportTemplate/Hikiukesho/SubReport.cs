using System;
using DevExpress.XtraReports.UI;

namespace HassyaAllrightCloud.Reports.ReportTemplate.Hikiukesho
{
    public partial class SubReport
    {
        public SubReport()
        {
            InitializeComponent();
        }

        //private void pageBreak1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    XRPageBreak control = sender as XRPageBreak;
        //    //int value = CurrentRowIndex + 1;
        //    int value = CurrentRowIndex;
        //    if (value % 12 == 0 && value != 0)
        //        control.Visible = true;
        //    else
        //        control.Visible = false;
        //}
    }
}
