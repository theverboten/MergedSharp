using System.Runtime.CompilerServices;
using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService(IOptions<CloudinarySettings> config)
        {

            Account myAccount = new Account
            (

            /* config.Value.CloudName,
             config.Value.ApiKey,
             config.Value.ApiSecret*/
            );

            _cloudinary = new Cloudinary(myAccount);

        }




        /*

        public async Task<VideoUploadResult> AddFileAsync(IFormFile file)
        {
            var uploadResult = new VideoUploadResult();

            if(file.Length > 0){
                using var stream = file.OpenReadStream();
                var uploadParams = new VideoUploadParams(){
                    File = new FileDescription(file.Name, stream),
                    Transformation = new Transformation().Flags($"attachment:"{file.Name})
                };
                Error -> uploadResult = await _cloudinary.UploadLargeAsync(uploadParams);
            }
            return uploadResult;
        }
    */
        public async Task<VideoUploadResult> AddAudioAsync(IFormFile file)
        {
            var uploadResult = new VideoUploadResult();
            /*  var uploadParams = new ImageUploadParams
              {
                  File = new FileDescription(@"C:\Users\hp\Desktop\SharpTest\API\ConvertedPdf.mp3"),
                  Folder = "SharpTest"
              };*/
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new VideoUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "SharpTest"
                };




                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }


            return uploadResult;
        }

        public async Task<DeletionResult> DeleteFileAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result;
        }
    }
}