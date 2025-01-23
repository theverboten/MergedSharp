using API.Interfaces;
using API.Models;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.TextToSpeech.V1;
using Grpc.Auth;


namespace API.Services
{
    public class StringConvertService : IStringConvertService
    {


        public string Base64Encode(string input)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(input);
            return System.Convert.ToBase64String(plainTextBytes);

        }

        public void StringToSpeech(string content, string pdfName)
        {
            Console.WriteLine("Google Cloud API is running");


            string proxy = $"-----BEGIN PRIVATE KEY-----\n{Environment.GetEnvironmentVariable("PRIVATE_KEY_ID_NEW_N")}\n-----END PRIVATE KEY-----\n"; //mělo by fungovat s funkčním credential jsonem-bez \n



            var newBigType = new CredentialsModel
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
            var serial64 = Base64Encode(serial);
            var buffer = Convert.FromBase64String(serial64);// input je base64 string

            Stream stream = new MemoryStream(buffer);
            GoogleCredential credential = GoogleCredential.FromStream(stream);

            var client = new TextToSpeechClientBuilder
            {
                Endpoint = TextToSpeechClient.DefaultEndpoint,
                ChannelCredentials = credential.ToChannelCredentials()
            }.Build();

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

            AudioConfig audioConfig = new AudioConfig
            {
                AudioEncoding = AudioEncoding.Mp3
            };
            SynthesizeSpeechResponse response = client.SynthesizeSpeech(input, voiceSelection, audioConfig);
            using (Stream output = File.Create(pdfName + ".mp3"))
            {
                response.AudioContent.WriteTo(output);
            }
            Console.WriteLine("Google Cloud API is finished ");
            Console.WriteLine("Is file directly?" + File.Exists(@"" + pdfName + ".mp3"));

            File.Move(@"" + pdfName + ".mp3", @"wwwroot/files/" + pdfName + ".mp3");
            Console.WriteLine("Was " + pdfName + ".mp3 moved? " + File.Exists(@"wwwroot/files/" + pdfName + ".mp3"));
        }

        public void DeleteSpeech(string fileName)
        {
            File.Delete(@"wwwroot/files/" + fileName + ".mp3");
            Console.WriteLine("Was " + fileName + ".mp3 deleted? " + !File.Exists(@"wwwroot/files/" + fileName + ".mp3"));

        }


    }
}