using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class ResponseModel
    {
        public long id { get; set; }

        public string pdfName { get; set; }
        public string content { get; set; }
    }
}