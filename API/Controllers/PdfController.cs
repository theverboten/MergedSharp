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

        private readonly HttpClient _httpClient;

        private readonly IStringConvertService _stringConvertService;

        private readonly ITestingService _testingService;

        private readonly IPdfStringService _pdfStringService;

        public PdfController(IPdfSpeechService pdfSpeechService, Client client, GoogleService googleService, HttpClient httpClient, IStringConvertService stringConvertService, ITestingService testingService, IPdfStringService pdfStringService)
        {
            _googleService = googleService;
            _client = client;
            _httpClient = httpClient;
            _pdfSpeechService = pdfSpeechService;
            _stringConvertService = stringConvertService;
            _testingService = testingService;
            _pdfStringService = pdfStringService;

        }

        [HttpGet("tested-pdf-to-speech-sync")]
        public void PdfOrderedConvertion()
        {

            _pdfSpeechService.OrderedPdfConvertion();
            Console.WriteLine("Ordered pdf converted successfully in dotnet");


            Ok();

        }
        [HttpGet("pdf-to-speech-sync")]
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
                Console.WriteLine("Pdf has too many pages. Limit is 3.");

                return BadRequest();

            }
            ;

        }



        [HttpGet("does-file-exist/{name}")]

        public bool DoesFileExist(string name)
        {

            var boolean = _testingService.FileExistTest($"{name}.mp3");
            Console.WriteLine(boolean);

            return boolean;
        }



        [HttpGet]
        [Route("DownloadByStream/{filename}")]
        public async Task<IActionResult> Download(string filename)
        {
            await Task.Delay(1000);
            string fileName = filename.Replace(".pdf", String.Empty); ;
            Console.WriteLine("DownloadByStream: " + fileName);

            /*  Stream stream = await _httpClient.GetStreamAsync(@"wwwroot/files/" + fileName + ".mp3");*/
            Stream stream = await _httpClient.GetStreamAsync(@"https://ctecka.fly.dev/files/" + fileName + ".mp3");// pokus s https protokolem


            if (stream == null)

                return NotFound(); // vrátí NotFoundResult

            Console.WriteLine("Download of " + fileName + " is successful");

            return File(stream, "audio/mpeg", fileName + ".mp3"); // vrátí FileStreamResult




        }

        [HttpGet]
        [Route("DownloadTestFile")]
        public async Task<IActionResult> DownloadTestFile()
        {
            await Task.Delay(1000);
            Console.WriteLine("DownloadTestFile: HelloWorld.pdf");

            /*  Stream stream = await _httpClient.GetStreamAsync(@"wwwroot/files/" + fileName + ".mp3");*/
            Stream stream = await _httpClient.GetStreamAsync(@"https://ctecka.fly.dev/files/HelloWorld.pdf");


            if (stream == null)
                return NotFound(); // vrátí NotFoundResult

            Console.WriteLine("Download of HelloWorld.pdf is successful");

            return File(stream, "application/pdf", "HelloWorld.pdf"); // vrátí FileStreamResult




        }

        [HttpDelete]
        [Route("Delete/{filename}")]
        public async Task<IActionResult> Delete(string filename)
        {
            await Task.Delay(100);

            /* string fileName = filename.Replace(".pdf", String.Empty); ;*/
            _stringConvertService.DeleteSpeech(filename);

            return Ok();

        }

        [HttpGet]
        [Route("complete-download/{filename}")]

        public async Task<IActionResult> CompleteDownload(string filename)
        {

            await Task.Delay(500);
            await Download(filename);
            await Delete(filename);

            return Ok();


        }


        [HttpGet]
        [Route("get-project-name")]
        public void GetProjectName()
        {
            string projectName = System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString();
            Console.WriteLine("Project name is: " + projectName);
        }

    }

}