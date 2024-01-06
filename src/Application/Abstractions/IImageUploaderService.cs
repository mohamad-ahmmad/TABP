using Microsoft.AspNetCore.Http;

namespace Application.Abstractions;

public interface IImageUploaderService
{
    Task<IEnumerable<string>> UploadImageAsync(IEnumerable<IFormFile> images);
}

