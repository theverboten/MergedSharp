using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using static API.Entities.ConvertedPdf;

namespace API.Interfaces
{
    public interface IStringConvertService
    {
        void StringToSpeech(string input, string pdfName);

        void DeleteSpeech(string fileName);
    }
}