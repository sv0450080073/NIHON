using System;
using System.Drawing;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;

namespace HassyaAllrightCloud.Reports
{
    public partial class ReceiptOutputReport2A4
    {
        public ReceiptOutputReport2A4()
        {
            InitializeComponent();
        }

        public void SetImageSource(Image image)
        {
            this.pictureBox1.ImageSource = new ImageSource(image);
        }
    }
}
