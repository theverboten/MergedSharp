using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Google.Cloud.TextToSpeech.V1;
using static API.Entities.ConvertedPdf;

namespace API.Services
{
    public class StringConvertService : IStringConvertService
    {

        public void StringToSpeech(string content, string pdfName)
        {
            Console.WriteLine("Google Cloud API is running");


            TextToSpeechClient client = TextToSpeechClient.Create();
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
            /* File.Delete(@"C:\Users\hp\Desktop\SharpTest\API\wwwroot\files\" + pdfName + ".mp3");*/
            /*  File.Move(@"C:\Users\hp\Desktop\SharpTest\API\" + pdfName + ".mp3", @"C:\Users\hp\Desktop\SharpTest\API\wwwroot\files\" + pdfName + ".mp3");*/
            File.Move(@"../API/" + pdfName + ".mp3", @"../API/wwwroot/files/" + pdfName + ".mp3");
            Console.WriteLine("Google Cloud API is finished ");
        }

        public void DeleteSpeech(string fileName)
        {
            /* File.Delete(@"C:\Users\hp\Desktop\SharpTest\API\wwwroot\files\" + fileName + ".mp3");*/
            File.Delete(@"../API/wwwroot/files/" + fileName + ".mp3");
        }

    }
}