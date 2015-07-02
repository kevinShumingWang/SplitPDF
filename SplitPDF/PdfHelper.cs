using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SplitPDF
{
    public class PdfHelper
    {
        public List<Image> GetImages(string filePath)
        {
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(filePath);
            return GetImages(doc);
        }
        public List<Image> GetImages(PdfDocument doc)
        {
            var listImage = new List<Image>();
            for (int i = 0; i < doc.Pages.Count; i++)
            {
                // Get an object of Spire.Pdf.PdfPageBase
                PdfPageBase page = doc.Pages[i];
                // Extract images from Spire.Pdf.PdfPageBase
                Image[] images = page.ExtractImages();
                if (images != null && images.Length > 0)
                {
                    listImage.AddRange(images);
                }
            }
            return listImage;
        }
    }
}
