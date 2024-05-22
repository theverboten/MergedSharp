using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.TextToSpeech.V1;

namespace API.Services
{
    public class GoogleService
    {

        public void TextSpeechService()
        {
            TextToSpeechClient client = TextToSpeechClient.Create();
            // The input can be provided as text or SSML.
            SynthesisInput input = new SynthesisInput
            {
                Text = "Hello there."
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
            using (Stream output = File.Create("sample.mp3"))
            {
                // response.AudioContent is a ByteString. This can easily be converted into
                // a byte array or written to a stream.
                response.AudioContent.WriteTo(output);
            }

            Console.WriteLine("Finished ");
        }

    }
}
