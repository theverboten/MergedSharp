using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using API.Interfaces;
using iText.Kernel.Utils.Annotationsflattening;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Http.HttpResults;




namespace API.Services
{
    public class DownloadService : IDownloadService
    {   /*
        public async Task<IActionResult> DownloadFile(string filename)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", filename);

            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(filepath, out var contentType))
            {
                contentType = "application/octet-stream";
            }


            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return            /* return System.IO.File(bytes, contentType, Path.GetFileName(filepath));*/
        public Task<IActionResult> DownloadFile(string filename)
        {
            throw new NotImplementedException();
        }









        public async void PdfClientDownload()
        {
            await Task.Delay(1000);
            var httpClient = new HttpClient();/*
            var responseStream = await httpClient.GetStreamAsync("http://localhost:5059/api/Pdf/download-from-path");
            using var fileStream = new FileStream(@"C:/Users/hp/Desktop/SharpTest/API/ConvertedPdf.mp3", FileMode.Create);
            responseStream.CopyTo(fileStream);*/

            var urlText = "http://localhost:5059/files/text.txt";

            /*  var textContext = await httpClient.GetStringAsync(urlText);
              Console.WriteLine(textContext);*/
            using (var client = new HttpClient())
            {
                using (var s = client.GetStreamAsync(urlText))
                {
                    using (var fs = new FileStream("text.txt", FileMode.OpenOrCreate)) { s.Result.CopyTo(fs); }
                }
            }

        }

        public FileStreamResult PdfDownload(string path)

        {
            var filename = System.IO.Path.GetFileName(path);
            var mimeType = "application/octet-stream";
            var stream = System.IO.File.OpenRead(path);

            return new FileStreamResult(stream, mimeType) { FileDownloadName = filename };
        }

    }
}