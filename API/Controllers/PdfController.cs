using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;


namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdfController : ControllerBase
    {

        private readonly IPdfSpeechService _pdfSpeechService;
        private readonly Client _client;

        private readonly GoogleService _googleService;
        private readonly IDownloadService _downloadService;

        private readonly HttpClient _httpClient;

        private readonly IStringConvertService _stringConvertService;

        private readonly ITestingService _testingService;

        private readonly IPdfStringService _pdfStringService;

        public PdfController(IPdfSpeechService pdfSpeechService, Client client, GoogleService googleService, IDownloadService downloadService, HttpClient httpClient, IStringConvertService stringConvertService, ITestingService testingService, IPdfStringService pdfStringService)
        {
            _downloadService = downloadService;
            _googleService = googleService;
            _client = client;
            _httpClient = httpClient;
            _pdfSpeechService = pdfSpeechService;
            _stringConvertService = stringConvertService;
            _testingService = testingService;
            _pdfStringService = pdfStringService;

        }

        /*  [HttpGet("pdf-to-speech")]
          public void PdfConvertion()
          {

              _pdfSpeechService.PdfConvertion();
              Console.WriteLine("Pdf converted successfully in dotnet");
              Ok();
          }*/
        [HttpGet("tested-pdf-to-speech-sync")]
        /* [HttpGet("pdf-to-speech-sync")]*/
        public void PdfOrderedConvertion()
        {

            _pdfSpeechService.OrderedPdfConvertion();
            Console.WriteLine("Ordered pdf converted successfully in dotnet");


            Ok();

        }
        [HttpGet("pdf-to-speech-sync")] // prohodil jsem url
        /* [HttpGet("tested-pdf-to-speech-sync")]*/
        public async Task<IActionResult> PdfTestedConvertion()
        {


            ConvertedPdf value = await _pdfSpeechService.DoClient();

            if (_pdfStringService.IsPdfAcceptable())
            {
                _stringConvertService.StringToSpeech(value.Content, value.PdfName);
                Console.WriteLine("Pdf converted successfully in DotNet.");
                return Ok();
            }
            else
            {
                Console.WriteLine("Pdf has too much pages. Limit is 3.");
                return BadRequest();

            };







        }

        /*
        [HttpGet("base64-client")]
        public void Base64ClientTest()
        {

            _pdfSpeechService.Base64Client();
            Console.WriteLine("Client was triggered in controller");
        }
        
        [HttpGet("get-base64")]
        public async Task Base64Convertion()
        {
            await _pdfSpeechService.Base64Convertion(_client._Client);
            Ok();
            /* 
            await _pdfSpeechService.Base64Convertion(_client);
            string? pdfString = await _client._Client.GetAsync("/");
            Ok();*/
        /*  } */          /*
           [HttpGet("text-to-speech-test")]
           public void TextSpeechTest()
           {
               _googleService.TextSpeechService();
               Console.WriteLine("Text to speech API Worked!");
           }  */
        /*
        [HttpGet("download-from-path")]

        public void PathDownload()
        {/*
            string path = @"C:/Users/hp/Desktop/SharpTest/API/ConvertedPdf.mp3";
            _downloadService.PdfDownload(path);
            Console.WriteLine("API is functional");*/
        /*   }*/
        /*
        [HttpGet("client")]

        public void HttpClientDownload()
        {
            _downloadService.PdfClientDownload();
            Console.WriteLine("HttpClient is functional");
        }*/
        /*
        [HttpGet("get-bool")]

        public bool BoolResponse()
        {
            return true;
        }
        */
        [HttpGet("does-file-exist/{name}")]

        public bool DoesFileExist(string name)
        {

            var boolean = _testingService.FileExistTest($"{name}.mp3");
            Console.WriteLine(boolean);
            return boolean;
        }

        /*
                [HttpGet]
                [Route("DownloadFile")]

                public async Task<IActionResult> DownloadFile(string filename)
                {
                    await Task.Delay(3000);
                    var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", filename);

                    var provider = new FileExtensionContentTypeProvider();

                    if (!provider.TryGetContentType(filepath, out var contentType))
                    {
                        contentType = "application/octet-stream";
                    }*/
        /* var httpClient = new HttpClient();
         var stream = await httpClient.GetStreamAsync()*/
        /*  var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
          return File(bytes, contentType, Path.GetFileName(filepath));*/
        /*
         var httpClient = new HttpClient();
         var stream = await httpClient.GetStreamAsync("http://localhost:5059/files/text.txt");
         var route = @"C:\Users\hp\Desktop\SharpTest\API\wwwroot\files\text.txt";
         using (var fileStream = File.Create(Path.Combine(route, "text.txt"))){
             stream.CopyTo(fileStream);
         }*//*
    }*/
        [HttpGet]
        [Route("DownloadByStream/{filename}")]
        public async Task<IActionResult> Download(string filename)
        {
            await Task.Delay(1000);
            string fileName = filename.Replace(".pdf", String.Empty); ;
            Console.WriteLine(fileName);
            Stream stream = await _httpClient.GetStreamAsync("http://localhost:5059/files/" + fileName + ".mp3");

            if (stream == null)
                return NotFound(); // returns a NotFoundResult with Status404NotFound response.

            /*return File(stream, "application/octet-stream", fileName); // returns a FileStreamResult*/
            Console.WriteLine("Download of " + fileName + " is successful");

            return File(stream, "audio/mpeg", fileName + ".mp3"); // returns a FileStreamResult*/




        }

        [HttpDelete]
        [Route("Delete/{filename}")]
        public async Task<IActionResult> Delete(string filename)
        {
            await Task.Delay(100);
            string fileName = filename.Replace(".pdf", String.Empty); ;
            _stringConvertService.DeleteSpeech(fileName);

            return Ok();

        }

        [HttpGet]
        [Route("complete-download/{filename}")]

        public async Task<IActionResult> CompleteDownload(string filename)
        {

            await Task.Delay(500);

            await Download(filename);

            /*  await Delete(filename);*/

            return Ok();


        }

        /*
        public void HttpClientDownload()
        {
            _downloadService.PdfClientDownload();
            Console.WriteLine("HttpClient is functional");
            File.Delete(@"C:\Users\hp\Desktop\SharpTest\API\wwwroot\files\" + pdfName + ".mp3");
        }
         */




    }

}