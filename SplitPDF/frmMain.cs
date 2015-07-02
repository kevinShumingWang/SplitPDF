using Spire.Pdf;
using Spire.Pdf.General.Find;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SplitPDF
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            ofdSingleFile.ShowDialog();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ofdSingleFile.SafeFileName))
            {
                MessageBox.Show("Please select a pdf file");
                return;
            }
            var filePath = ofdSingleFile.FileName;
            var fileName = ofdSingleFile.SafeFileName;

            var ListImage = new List<Image>();
            PdfHelper helper = new PdfHelper();
            PdfDocument pdf = new PdfDocument(filePath);
            PdfPageBase pageOrigin = pdf.Pages[0];
            SizeF size = pageOrigin.Size;
            PdfTemplate template = pageOrigin.CreateTemplate();



            #region First Page
            PdfDocument doc = new PdfDocument();
            PdfPageBase page0 = doc.Pages.Add(new SizeF(288, 432), new PdfMargins(0));

            PdfPageBase page1 = doc.Pages.Add(new SizeF(288, 432), new PdfMargins(0));

            page1.Canvas.DrawTemplate(template, new PointF(38, 9));

            PdfTemplate templateNew = page1.CreateTemplate();
            PdfPageBase pageT = doc.Pages.Add(new SizeF(288, 432), new PdfMargins(0));
            pageT.Canvas.DrawTemplate(templateNew, new PointF(-40, -9));

            #endregion

            ListImage = helper.GetImages(pdf);
            #region Second Page
            if (ListImage.Count == 2)
            {
                PdfPageBase page2 = doc.Pages.Add(new SizeF(288, 432), new PdfMargins(0));

                // page.Rotation = PdfPageRotateAngle.RotateAngle90;
                ListImage[1].RotateFlip(RotateFlipType.Rotate90FlipNone);

                #region cut image
                System.Drawing.Rectangle cropArea = new System.Drawing.Rectangle(0, 0, 800, 1204);
                var img = ListImage[1];
                System.Drawing.Bitmap bmpImage = new System.Drawing.Bitmap(img);
                System.Drawing.Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
                #endregion
                PdfImage image = PdfImage.FromImage(bmpCrop);
                page2.Canvas.DrawImage(image, 0, 0, page2.Canvas.ClientSize.Width, page2.Canvas.ClientSize.Height);
                doc.Pages.RemoveAt(0);
                doc.Pages.RemoveAt(1);
            }
            else
            {
                doc.Pages.RemoveAt(0);
            }
            #endregion
            var newFile = filePath.Remove(filePath.LastIndexOf('.'), 4) + "_cut"+DateTime.Now.ToFileTime()+".pdf";
            doc.SaveToFile(newFile);
            doc.Close();
            lblMsg.Text = "File has been save as " + newFile;
            System.Diagnostics.Process.Start(newFile);
        }

        private void ofdSingleFile_FileOk(object sender, CancelEventArgs e)
        {
            txtFilePath.Text = ofdSingleFile.SafeFileName;
            lblMsg.Text = "";
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Coming soon");
        }
    }
}
