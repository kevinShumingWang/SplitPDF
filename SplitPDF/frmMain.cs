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
            Control.CheckForIllegalCrossThreadCalls = false;
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
            btnRun.Enabled = false;

            Task.Factory.StartNew(() =>
            {
                var filePath = ofdSingleFile.FileName;
                var helper = new PdfHelper();
                var pdf = new PdfDocument(filePath);
                var pdfTemp = new PdfDocument();
                var docWillSave = new PdfDocument();
                var totalPage = pdf.Pages.Count;
                pbSplitProgress.Maximum = totalPage;
                var curPage = 0;
                foreach (PdfPageBase itemPage in pdf.Pages)
                {
                    curPage++;
                    #region Lable one
                    var pageOrigin = itemPage;
                    var template = pageOrigin.CreateTemplate();
                    var pageTemp = pdfTemp.Pages.Add(PdfHelper.SizeF4x6, new PdfMargins(0));
                    pageTemp.Canvas.DrawTemplate(template, new PointF(38, 9));
                    var templateNew = pageTemp.CreateTemplate();
                    var page = docWillSave.Pages.Add(PdfHelper.SizeF4x6, new PdfMargins(0));
                    page.Canvas.DrawTemplate(templateNew, new PointF(-40, -9));


                    PdfGraphicsState state = page.Canvas.Save();
                    PdfStringFormat leftAlignment = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                    PdfFont font = new PdfFont(PdfFontFamily.Helvetica, 20f);
                    PdfSolidBrush brush = new PdfSolidBrush(Color.Blue);
                    page.Canvas.RotateTransform(-90);
                    page.Canvas.DrawString(txtText.Text, font, brush, new PointF(-250, 25), leftAlignment);
                    //  page.Canvas.Restore(state);



                    pdfTemp.Pages.RemoveAt(0);
                    #endregion

                    #region Lable two
                    var ListImage = itemPage.ExtractImages();
                    #region Second Page
                    PdfPageBase page2 = docWillSave.Pages.Add(new SizeF(288, 432), new PdfMargins(0));
                    ListImage[1].RotateFlip(RotateFlipType.Rotate90FlipNone);

                    #region cut image
                    System.Drawing.Rectangle cropArea = new System.Drawing.Rectangle(0, 0, 800, 1204);
                    var img = ListImage[1];
                    System.Drawing.Bitmap bmpImage = new System.Drawing.Bitmap(img);
                    System.Drawing.Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
                    #endregion
                    PdfImage image = PdfImage.FromImage(bmpCrop);
                    page2.Canvas.DrawImage(image, 0, 0, page2.Canvas.ClientSize.Width, page2.Canvas.ClientSize.Height);
                    #endregion
                    #endregion
                    pbSplitProgress.Value = curPage ;
                }

                var newFile = filePath.Remove(filePath.LastIndexOf('.'), 4) + "_cut" + DateTime.Now.ToFileTime() + ".pdf";
                docWillSave.SaveToFile(newFile);
                docWillSave.Close();
                lblMsg.Text = "File has been save as " + newFile;
                System.Diagnostics.Process.Start(newFile);
            });

          
          
        }

        private void ofdSingleFile_FileOk(object sender, CancelEventArgs e)
        {
            txtFilePath.Text = ofdSingleFile.SafeFileName;
            lblMsg.Text = "";
            pbSplitProgress.Value = 0;
            btnRun.Enabled = true;
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Coming soon");
        }
    }
}
