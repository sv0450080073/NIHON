using System.Drawing;
using DevExpress.XtraPrinting.Drawing;

namespace HassyaAllrightCloud.Reports
{
    public partial class NoReceiptOutputReport1A4
    {
        public NoReceiptOutputReport1A4()
        {
            InitializeComponent();
        }

        public void SetImageSource(Image image)
        {
            this.pictureBox1.ImageSource = new ImageSource(image);
        }
    }
}
