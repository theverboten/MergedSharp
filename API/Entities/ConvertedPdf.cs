using System.Buffers.Text;

namespace API.Entities
{
    public class ConvertedPdf
    {
        public int Id { get; set; }
        public string PdfName { get; set; }
        public string Content { get; set; }

    }
}