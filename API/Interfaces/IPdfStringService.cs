using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using iText.Commons.Utils;

namespace API.Interfaces
{
    public interface IPdfStringService
    {
        Task<String> ExtractTextFromPdfAsync();
        string ExtractTextFromPdf();

        string FixedExtractTextFromPdf();

        bool IsPdfAcceptable();
    }
}