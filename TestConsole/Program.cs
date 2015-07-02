using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            PdfDocument document = new PdfDocument(PdfConformanceLevel.Pdf_A1B);
            PdfPageBase page = document.Pages.Add();
          
            PdfPageBase page3 = document.Pages.Add();
            page3.Canvas.DrawString("Hello World2", new PdfFont(PdfFontFamily.Helvetica, 30f), new PdfSolidBrush(Color.Black), 10, 10);
            //save to PDF document
            document.Pages[1].Rotation = PdfPageRotateAngle.RotateAngle180;
            document.Pages.RemoveAt(0);
            document.SaveToFile("FromHTML.pdf", FileFormat.PDF);
            document.Close();
          
        }
    }
}
