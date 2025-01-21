using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Google.Cloud.TextToSpeech.V1;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace API.Services
{
    public class PdfStringService : IPdfStringService
    {
        /* public string ExtractTextFromPdf()
         {
             string path = @"C:/Users/hp/Desktop/HelloWorld.pdf";

             using (PdfReader reader = new PdfReader(path))
             {
                 StringBuilder text = new StringBuilder();
                 PdfDocument doc = new PdfDocument(reader);
                 int numberOfPages = doc.GetNumberOfPages();

                 for (int i = 1; i <= numberOfPages; i++)
                 {
                     var page = doc.GetPage(i);

                     text.Append(PdfTextExtractor.GetTextFromPage(page));
                 }
                 string rec = text.ToString();

                 Console.WriteLine(rec);

                 return rec;
             }
         }*/
        public string responsePath = @"wwwroot/files/response.pdf";

        public async Task<String> ExtractTextFromPdfAsync()
        {
            await Task.Delay(10000);
            //string path = @"../API/wwwroot/files/response.pdf";

            string path = responsePath;



            using (PdfReader reader = new PdfReader(path))
            {

                ConvertedPdf convertedPdf = new ConvertedPdf();
                StringBuilder text = new StringBuilder();
                PdfDocument doc = new PdfDocument(reader);
                int numberOfPages = doc.GetNumberOfPages();

                for (int i = 1; i <= numberOfPages; i++)
                {
                    var page = doc.GetPage(i);

                    text.Append(PdfTextExtractor.GetTextFromPage(page));
                }
                int number = 1;
                convertedPdf.Id = number;
                convertedPdf.Content = text.ToString();

                Console.WriteLine("In pdf.Content is " + convertedPdf.Content);

                return await Task.FromResult(convertedPdf.Content);
            }
        }

        public string ExtractTextFromPdf()
        {
            //string path = @"../API/wwwroot/files/response.pdf";

            string path = responsePath;


            using (PdfReader reader = new PdfReader(path))
            {

                ConvertedPdf convertedPdf = new ConvertedPdf();
                StringBuilder text = new StringBuilder();
                PdfDocument doc = new PdfDocument(reader);
                int numberOfPages = doc.GetNumberOfPages();



                for (int i = 1; i <= numberOfPages; i++)
                {
                    var page = doc.GetPage(i);

                    text.Append(PdfTextExtractor.GetTextFromPage(page));
                }
                int number = 1;
                convertedPdf.Id = number;
                convertedPdf.Content = text.ToString();

                string response = convertedPdf.Content;
                /*  Console.WriteLine(response);*/

                return response;
            }
        }

        public bool IsPdfAcceptable()
        {
            // string path = @"../API/wwwroot/files/response.pdf";
            string path = responsePath;


            using (PdfReader reader = new PdfReader(path))
            {

                ConvertedPdf convertedPdf = new ConvertedPdf();
                StringBuilder text = new StringBuilder();
                PdfDocument doc = new PdfDocument(reader);
                int numberOfPages = doc.GetNumberOfPages();

                if (numberOfPages > 3)
                {
                    return false;
                }
                else
                {
                    return true;
                }



            }
        }

        public string FixedExtractTextFromPdf()
        {
            //string path = @"../API/wwwroot/files/response.pdf";
            string path = responsePath;


            using (PdfReader reader = new PdfReader(path))
            {

                ConvertedPdf convertedPdf = new ConvertedPdf();
                StringBuilder text = new StringBuilder();
                PdfDocument doc = new PdfDocument(reader);
                int numberOfPages = doc.GetNumberOfPages();

                for (int i = 1; i <= numberOfPages; i++)
                {
                    var page = doc.GetPage(i);

                    text.Append(PdfTextExtractor.GetTextFromPage(page));
                }
                int number = 1;
                convertedPdf.Id = number;
                convertedPdf.Content = text.ToString();

                Console.WriteLine("String service is running");
                string response = convertedPdf.Content;
                Console.WriteLine(response);

                return response;
            }
        }
    }
}