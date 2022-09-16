using System;
using System.Drawing;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;

namespace HassyaAllrightCloud.Reports
{
    public partial class PaymentRequestPreviewReport2A4
    {
        public PaymentRequestPreviewReport2A4()
        {
            InitializeComponent();
        }

        public void SetImageSource(Image image)
        {
            this.pictureBox1.ImageSource = new ImageSource(image);
        }
    }
}
