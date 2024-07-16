using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using iText.Pdfa.Logs;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json.Linq;
using static API.Entities.ConvertedPdf;
using System.Net.Http.Json;
using API.Models;
using System.ComponentModel;



namespace API.Services
{


    public class PdfSpeechService : IPdfSpeechService
    {

        private readonly IPdfStringService _pdfStringService;
        private readonly IStringConvertService _stringConvertService;
        private readonly Client _client;
        private readonly IHttpClientFactory _httpClientFactory;

        public ResponseModel responseModel { get; set; }

        public string errorString { get; set; }

        static string memoryString = "";

        static string memoryPdfNameString = "";



        public PdfSpeechService(IPdfStringService pdfStringService, IStringConvertService stringConvertService, Client client, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _client = client;
            _stringConvertService = stringConvertService;
            _pdfStringService = pdfStringService;
        }
        public async Task PdfConvertion() // IGNOROVAT
        {
            /*  await Base64Convertion(_client._Client);  */
            await Base64Client();

            Console.WriteLine("Base64Convertion done for Client");
            var input = _pdfStringService.ExtractTextFromPdf();
            Console.WriteLine("Input from PdfSpeechService is " + input);
            _stringConvertService.StringToSpeech(input, input);
        }

        public async void OrderedPdfConvertion()
        {
            /*  var value = await Base64Client();*/
            /*   await Task.Delay(4000); */

            ConvertedPdf value = await DoClient();

            Console.WriteLine("Name from DoClient is " + value.PdfName);
            Console.WriteLine("Input from DoClient is " + value.Content);

            /* Console.WriteLine("Base64Convertion done for Client: " + value);*/
            /*  var input = _pdfStringService.ExtractTextFromPdf();*/
            _stringConvertService.StringToSpeech(value.Content, value.PdfName);


        }










        public async Task Base64Convertion(HttpClient httpClient)  // IGNOROVAT
        {

            /*  using HttpResponseMessage response = await httpClient.GetAsync("data/1");
              response.EnsureSuccessStatusCode();*/

            HttpResponseMessage response = await httpClient.GetAsync("data/1");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic data = JObject.Parse(jsonResponse);
            string base64BinaryStr = data.base64;
            string refinedBase64 = base64BinaryStr.Replace("data:application/pdf;base64,", String.Empty);
            byte[] bytes = Convert.FromBase64String(refinedBase64);
            File.WriteAllBytes(@"C:\Users\hp\Desktop\base64Response.pdf", bytes);
            Console.WriteLine("Done");



            /* Console.WriteLine($"{data.base64}\n");
             Console.WriteLine($"{data.id}\n");*/
        }
        public async Task Base64Postgres(HttpClient httpClient) //IGNOROVAT
        {
            httpClient.Timeout = TimeSpan.FromMinutes(5);
            HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, "api/Database/get-uploaded-pdf/1");
            request.Headers.IfModifiedSince = new DateTimeOffset(DateTime.Now);
            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic data = JObject.Parse(jsonResponse);
            string base64BinaryStr = data.content;

            if (base64BinaryStr == memoryString)
            {
                Console.WriteLine("Duplicate data from database!");
            }

            base64BinaryStr = memoryString;
            string refinedBase64 = base64BinaryStr.Replace("data:application/pdf;base64,", String.Empty);
            byte[] bytes = Convert.FromBase64String(refinedBase64);
            File.WriteAllBytes(@"C:\Users\hp\Desktop\base64Response.pdf", bytes);
            Console.WriteLine("Done, Postgres is successfull");



            /* Console.WriteLine($"{data.base64}\n");
             Console.WriteLine($"{data.id}\n");*/
        }

        public async Task<ConvertedPdf> Base64Client()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5059/api/Database/get-uploaded-pdf/1");
            var client = _httpClientFactory.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                responseModel = await response.Content.ReadFromJsonAsync<ResponseModel>();
                errorString = null;

            }
            else
            {
                errorString = $"There was an error getting data from database: {response.ReasonPhrase}";
            }
            Console.WriteLine("******");
            Console.WriteLine($"{responseModel.id}"); // Metoda funguje
            Console.WriteLine("******");
            /* await Task.Delay(2000);*/
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("11");
            dynamic data = JObject.Parse(jsonResponse);
            Console.WriteLine("22");
            string base64BinaryStr = data.content;
            Console.WriteLine("33");
            string refinedBase64 = base64BinaryStr.Replace("data:application/pdf;base64,", String.Empty);
            Console.WriteLine("44");
            byte[] bytes = Convert.FromBase64String(refinedBase64);
            Console.WriteLine("55");
            /*  File.WriteAllBytes("http://localhost:5059/files/response.pdf", bytes);*/
            File.WriteAllBytes(@"../API/wwwroot/files/response.pdf", bytes);
            Console.WriteLine("66");

            var input = _pdfStringService.ExtractTextFromPdf();
            /*  Console.WriteLine("V Pdf mezipaměti je " + input);*/
            /*  Console.WriteLine("V memory stringu bylo " + memoryString);*/
            memoryString = input;
            memoryPdfNameString = responseModel.pdfName;


            string refinedPdfNameString = memoryPdfNameString.Replace(".pdf", String.Empty);

            ConvertedPdf model = new ConvertedPdf { Id = 1, PdfName = refinedPdfNameString, Content = input };
            Console.WriteLine("V databázi bylo tohle PDF: " + refinedPdfNameString);
            /*  Console.WriteLine("V memory stringu je " + memoryString);*/

            return model;

            /*
            if (base64BinaryStr != memoryString)
            {
                memoryString = base64BinaryStr;

                Console.WriteLine("Data was not duplicate! Process continues!");

              */


            /*  string refinedBase64 = base64BinaryStr.Replace("data:application/pdf;base64,", String.Empty);
              byte[] bytes = Convert.FromBase64String(refinedBase64);


              /*  string result = System.Text.Encoding.UTF8.GetString(bytes);
              string result = BitConverter.ToString(bytes);
              Console.WriteLine("Bytes is " + result);

              File.WriteAllBytes(@"C:\Users\hp\Desktop\base64Response.pdf", bytes);
               Console.WriteLine($"{bytes}");
              var input = _pdfStringService.ExtractTextFromPdf();
              Console.WriteLine("V mezipaměti je " + input);
              Console.WriteLine("First");*/
            /*
            }
            else
            {
                Console.WriteLine("Data was duplicate! Process was stopped!");
            }*/
        }

        public async Task PdfConvertionAsync()
        {
            await Task.Delay(10000);
            var input = _pdfStringService.ExtractTextFromPdfAsync();
            /*   Console.WriteLine("Input from PdfSpeechService is " + input.Result);*/
            /*  _stringConvertService.StringToSpeech(input.Result, input.Result);*/

        }

        public async Task<ConvertedPdf> DoClient() // FUNKČNÍ!!!
        {

            var returnValue1 = await Base64Client();
            /*
            Console.WriteLine("Return value z 1) kola je " + returnValue1);
            Console.WriteLine("MemoryString je " + memoryString);*/

            if (returnValue1.Content == memoryString)
            {
                /* Console.WriteLine("Kolo Do/While: 2)");*/
                var returnValue2 = await Base64Client();

                return returnValue2;

            }

            else

            {
                return returnValue1;
            }


            /*  var returnValue2 = "";*/
            /*
            do
            {
                Console.WriteLine("Kolo Do/While: " + i + ")");
                i++;

                returnValue1 = await Base64Client();
                /* await Task.Delay(1000);
                 returnValue2 = await Base64Client();
                return returnValue1;





            }
            while (memoryString == returnValue1);*/

        }
    }
}