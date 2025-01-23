using API.Entities;
using API.Interfaces;
using Newtonsoft.Json.Linq;
using API.Models;



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

        public async void OrderedPdfConvertion()
        {

            ConvertedPdf value = await DoClient();

            Console.WriteLine("Name from DoClient is " + value.PdfName);
            Console.WriteLine("Input from DoClient is " + value.Content);

            _stringConvertService.StringToSpeech(value.Content, value.PdfName);


        }



        public async Task<ConvertedPdf> Base64Client()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://ctecka.fly.dev/api/Database/get-uploaded-pdf/1");
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
            File.WriteAllBytes(@"wwwroot/files/response.pdf", bytes);
            Console.WriteLine("66");

            var input = _pdfStringService.ExtractTextFromPdf();
            //  Console.WriteLine("V Pdf mezipaměti je " + input);
            memoryString = input;
            memoryPdfNameString = responseModel.pdfName;


            string refinedPdfNameString = memoryPdfNameString.Replace(".pdf", String.Empty);

            ConvertedPdf model = new ConvertedPdf { Id = 1, PdfName = refinedPdfNameString, Content = input };
            Console.WriteLine("V databázi bylo tohle PDF: " + refinedPdfNameString);
            //  Console.WriteLine("V memory stringu je " + memoryString);

            return model;
        }
        /*
        public async Task PdfConvertionAsync()
        {
            await Task.Delay(10000);
            var input = _pdfStringService.ExtractTextFromPdfAsync();
            //   Console.WriteLine("Input from PdfSpeechService is " + input.Result);
            //  _stringConvertService.StringToSpeech(input.Result, input.Result);

        }*/

        public async Task<ConvertedPdf> DoClient() // FUNKČNÍ!!!
        {

            var returnValue1 = await Base64Client();

            if (returnValue1.Content == memoryString)
            {
                // Console.WriteLine("Kolo Do/While: 2)");
                var returnValue2 = await Base64Client();
                return returnValue2;
            }
            else
            {
                return returnValue1;
            }
        }
    }
}