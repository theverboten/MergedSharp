using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Components.Forms;

namespace API.Interfaces
{
    public interface IPdfSpeechService
    {
        Task PdfConvertion();

        void OrderedPdfConvertion();


        Task PdfConvertionAsync();
        Task Base64Convertion(HttpClient httpClient);
        Task Base64Postgres(HttpClient httpClient);

        Task<ConvertedPdf> Base64Client();

        Task<ConvertedPdf> DoClient();

    }
}