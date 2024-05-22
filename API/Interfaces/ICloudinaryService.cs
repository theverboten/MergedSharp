
using CloudinaryDotNet.Actions;

namespace API.Interfaces
{
    public interface ICloudinaryService
    {
        Task<VideoUploadResult> AddAudioAsync(IFormFile file);

        Task<DeletionResult> DeleteFileAsync(string publicId);
    }
}