using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Interfaces;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CloudController : ControllerBase
    {
        private readonly ICloudinaryService _cloudinaryService;

        public CloudController(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;

        }


        [HttpPost("add-audio")]

        public async Task<ActionResult<String>> AddAudioAsync(IFormFile file)
        {
            var result = await _cloudinaryService.AddAudioAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var audio = new
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            return audio.PublicId;
            /* PublicId = SharpTest/bcndiko2ayuj3ywbxhsd*/
        }

        [HttpDelete("delete-audio")]

        public async Task<String> DeleteAudio(string audioId)
        {
            var result = await _cloudinaryService.DeleteFileAsync(audioId);

            return "Deleted";
        }

    }
}