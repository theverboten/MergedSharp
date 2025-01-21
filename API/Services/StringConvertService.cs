using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using API.Models;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.TextToSpeech.V1;
using Grpc.Auth;
using Grpc.Core;
using Grpc.Net.Client;
using Newtonsoft.Json;
using static API.Entities.ConvertedPdf;


namespace API.Services
{
    public class StringConvertService : IStringConvertService
    {   /*
        public class TypeJson
        {
            //  public string clientEmail { get; set; }
            public string private_key { get; set; }

        }*/

        public class BigTypeJson
        {
            public string type { get; set; }
            public string project_id { get; set; }
            public string private_key_id { get; set; }
            public string private_key { get; set; }
            public string client_email { get; set; }
            public string client_id { get; set; }
            public string auth_uri { get; set; }
            public string token_uri { get; set; }
            public string auth_provider_x509_cert_url { get; set; }
            public string client_x509_cert_url { get; set; }
            public string universe_domain { get; set; }

        }


        public string Base64Encode(string input)
        {


            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(input);
            return System.Convert.ToBase64String(plainTextBytes);

        }
        /*
        public ServiceAccountCredential GetCredential()
        { 
            string privateKey64 = Base64Encode(Environment.GetEnvironmentVariable("PRIVATE_KEY_CLEAR"));
            return new ServiceAccountCredential(
                   new ServiceAccountCredential.Initializer(Environment.GetEnvironmentVariable("CLIENT_EMAIL"))
                   {
                       Scopes = new[]
                {
                 "https://texttospeech.googleapis.com",
                 "https://www.googleapis.com/auth/cloud-platform"
                },
                       User = "medikacerychlaakce@gmail.com",
                   }.FromPrivateKey("-----BEGIN PRIVATE KEY-----\n" + privateKey64 + "\n-----END PRIVATE KEY-----\n")
             );
        }*/

        public void StringToSpeech(string content, string pdfName)
        {
            Console.WriteLine("Google Cloud API is running");


            string proxy = $"-----BEGIN PRIVATE KEY-----\n{Environment.GetEnvironmentVariable("PRIVATE_KEY_ID_NEW_N")}\n-----END PRIVATE KEY-----\n"; //mělo by fungovat s funkčním credential jsonem-bez \n



            var newBigType = new BigTypeJson
            {
                type = Environment.GetEnvironmentVariable("TYPE_NEW"),
                project_id = Environment.GetEnvironmentVariable("PROJECT_ID_NEW"),
                private_key_id = Environment.GetEnvironmentVariable("PRIVATE_KEY_ID_NEW"),
                private_key = proxy,
                client_email = Environment.GetEnvironmentVariable("CLIENT_EMAIL_NEW"),
                client_id = Environment.GetEnvironmentVariable("CLIENT_ID_NEW"),
                auth_uri = Environment.GetEnvironmentVariable("AUTH_URI_NEW"),
                token_uri = Environment.GetEnvironmentVariable("TOKEN_URI_NEW"),
                auth_provider_x509_cert_url = Environment.GetEnvironmentVariable("AUTH_PROVIDER_NEW"),
                client_x509_cert_url = Environment.GetEnvironmentVariable("CLIENT_CERT_URL_NEW"),
                universe_domain = Environment.GetEnvironmentVariable("UNIVERSE_DOMAIN_NEW"),

            };


            var serial = Newtonsoft.Json.JsonConvert.SerializeObject(newBigType);
            //  var serial2 = Newtonsoft.Json.JsonConvert.SerializeObject(type);
            // TypeJson deserial = JsonConvert.DeserializeObject<TypeJson>(serial2);
            var serial64 = Base64Encode(serial);



            //  Console.WriteLine("Deserial: " + deserial.private_key);
            /*using MemoryStream ms = new MemoryStream(deserial);
            TypeJson ex = JsonSerializer.Deserialize<TypeJson>(ms);
             Console.WriteLine(ex.private_key);*/



            /*
            var credential = GoogleCredential.FromFile(@"wwwroot/files/credentials.json").CreateScoped(TextToSpeechClient.DefaultScopes);
            var channel = new Grpc.Core.ChannelCredentials(TextToSpeechClient.DefaultEndpoint.ToString(), credential.ToChannelCredentials());*/
            // Instantiates a client
            /*řešení buffer ze stacku: GoogleCredential.FromFile*/
            // var credential = GoogleCredential.FromServiceAccountCredential(GetCredential()); : U tohoto řešení se pokaždé dopracuju k Tag 13 erroru
            //var buffer = Environment.GetEnvironmentVariable("PRIVATE_KEY_CLEAR");

            // byte[] bytes = Encoding.UTF8.GetBytes(type.privateKey);
            var buffer = Convert.FromBase64String(serial64);// input je base64 string
            Stream stream = new MemoryStream(buffer);
            GoogleCredential credential = GoogleCredential.FromStream(stream);
            /*
            string privateKey64 = Base64Encode();
            //  string privateKey64 = Base64Encode("-----BEGIN PRIVATE KEY-----\n" + Environment.GetEnvironmentVariable("PRIVATE_KEY_CLEAR") + "\n-----END PRIVATE KEY-----\n");
            //  Console.WriteLine("Začátek :" + privateKey64);
            var buffer = Convert.FromBase64String(privateKey64);
            Stream stream = new MemoryStream(buffer);
            GoogleCredential credential = GoogleCredential.FromStream(stream);*/
            /* var credential = GoogleCredential.FromFile(@"wwwroot/files/credentials.json");*/
            // or if you have access to the content only
            // var credential = GoogleCredential.FromJson(json);
            var client = new TextToSpeechClientBuilder
            {
                Endpoint = TextToSpeechClient.DefaultEndpoint,
                ChannelCredentials = credential.ToChannelCredentials()
            }.Build();
            //**** ****
            /* string credential_path = @"wwwroot/files/credentials.json";
             System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);*/
            //**** ****
            // TextToSpeechClient client = TextToSpeechClient.Create();
            // The input can be provided as text or SSML.
            /*    Console.WriteLine("    Input is " + content);*/
            SynthesisInput input = new SynthesisInput
            {
                Text = content
            };
            // You can specify a particular voice, or ask the server to pick based
            // on specified criteria.
            VoiceSelectionParams voiceSelection = new VoiceSelectionParams
            {
                LanguageCode = "en-US",
                SsmlGender = SsmlVoiceGender.Female
            };
            // The audio configuration determines the output format and speaking rate.
            AudioConfig audioConfig = new AudioConfig
            {
                AudioEncoding = AudioEncoding.Mp3
            };
            SynthesizeSpeechResponse response = client.SynthesizeSpeech(input, voiceSelection, audioConfig);
            using (Stream output = File.Create(pdfName + ".mp3"))
            {
                // response.AudioContent is a ByteString. This can easily be converted into
                // a byte array or written to a stream.
                response.AudioContent.WriteTo(output);
            }
            Console.WriteLine("Google Cloud API is finished ");
            Console.WriteLine("Is file directly?" + File.Exists(@"" + pdfName + ".mp3"));//true
                                                                                         // Console.WriteLine("Is file in app?" + File.Exists(@"app/" + pdfName + ".mp3"));
                                                                                         // Console.WriteLine("Is file in app/wwwroot?" + File.Exists(@"app/wwwroot/" + pdfName + ".mp3"));
            /* File.Delete(@"C:\Users\hp\Desktop\SharpTest\API\wwwroot\files\" + pdfName + ".mp3");*/
            /*  File.Move(@"C:\Users\hp\Desktop\SharpTest\API\" + pdfName + ".mp3", @"C:\Users\hp\Desktop\SharpTest\API\wwwroot\files\" + pdfName + ".mp3");*/
            // File.Move(@"../API/" + pdfName + ".mp3", @"../API/wwwroot/files/" + pdfName + ".mp3");
            /*
             try
             {
                 File.Move(@"app/" + pdfName + ".mp3", @"files/" + pdfName + ".mp3");
                 Console.WriteLine("Trycatch succeeded");
             }
             catch
             {
                 Console.WriteLine("Trycatch failed");
             }*/
            File.Move(@"" + pdfName + ".mp3", @"wwwroot/files/" + pdfName + ".mp3");
            Console.WriteLine("Was " + pdfName + ".mp3 moved? " + File.Exists(@"wwwroot/files/" + pdfName + ".mp3"));
        }

        public void DeleteSpeech(string fileName)
        {
            /* File.Delete(@"C:\Users\hp\Desktop\SharpTest\API\wwwroot\files\" + fileName + ".mp3");*/
            // File.Delete(@"../API/wwwroot/files/" + fileName + ".mp3");
            File.Delete(@"wwwroot/files/" + fileName + ".mp3");
            Console.WriteLine("Was " + fileName + ".mp3 deleted? " + !File.Exists(@"wwwroot/files/" + fileName + ".mp3"));

        }


    }
}