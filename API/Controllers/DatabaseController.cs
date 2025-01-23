using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseController : ControllerBase
    {
        private readonly DataContext _context;

        public DatabaseController(DataContext context)
        {
            _context = context;
        }


        [HttpGet("{id}")]
        public ActionResult<ConvertedPdf> GetPdfById(int id)
        {
            var pdf = _context.StoredPdf.Find(id);
            if (pdf == null)
            {
                return NotFound();
            }
            return pdf;
        }

        [HttpPost("post-data")]

        public ActionResult<ConvertedPdf> Create([FromBody] ConvertedPdf convertedPdf)
        {
            _context.StoredPdf.Add(convertedPdf);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetPdfById), new { id = convertedPdf.Id, }, convertedPdf);
        }


        [HttpPut("upload-pdf")]

        public ActionResult<ConvertedPdf> UpdatePdfWithoutId(string information)
        {
            Console.WriteLine("Information was received");
            Console.WriteLine(information);
            ConvertedPdf databasePdf = new ConvertedPdf { Id = 1, Content = information };
            _context.StoredPdf.Update(databasePdf);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("upload-pdf-json-with-id/{id}")]

        public ActionResult<ConvertedPdf> UpdatePdfWithId(ConvertedPdf information, int id)
        {
            Console.WriteLine("Information was received");
            Console.WriteLine(id);
            Console.WriteLine("Information was received");
            ConvertedPdf databasePdf = new ConvertedPdf { Id = id, PdfName = information.PdfName, Content = information.Content };
            Console.WriteLine(databasePdf.PdfName);
            _context.StoredPdf.Update(databasePdf);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("database-check/{id}")]

        public ActionResult<ConvertedPdf> DatabaseCheck(ConvertedPdf information, int id)
        {
            Console.WriteLine("Information was receive");
            Console.WriteLine(id);
            Console.WriteLine("Information was received");
            ConvertedPdf databasePdf = new ConvertedPdf { Id = id, PdfName = information.PdfName, Content = information.Content };
            Console.WriteLine(databasePdf.PdfName);
            _context.StoredPdf.Update(databasePdf);
            _context.SaveChanges();
            return Ok();
        }



        [HttpGet("get-uploaded-pdf/{id}")]
        public ActionResult<ConvertedPdf> GetUploadedPdfById(int id)
        {
            var pdf = _context.StoredPdf.Find(id);
            if (pdf == null)
            {
                return NotFound();
            }
            return pdf;
        }


    }
}