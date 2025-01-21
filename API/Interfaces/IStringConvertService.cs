using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Google.Apis.Auth.OAuth2;
using static API.Entities.ConvertedPdf;

namespace API.Interfaces
{
    public interface IStringConvertService
    {
        void StringToSpeech(string input, string pdfName);

        void DeleteSpeech(string fileName);

        // ServiceAccountCredential GetCredential();
        string Base64Encode(string input);
    }
}