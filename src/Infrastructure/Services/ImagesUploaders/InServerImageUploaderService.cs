using Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Uploaders;

public class InServerImageUploaderService : IImageUploaderService
{

    private string _webRootPath =null!;
    public void SetWebRootPath(string webRootPath)
    {
        _webRootPath = webRootPath;
    }
    public async Task<string> StoreImageToServer(IFormFile image)
    {
        var imgExt = Path.GetExtension(image.FileName);
        var imgName = Guid.NewGuid() + imgExt;
        string filePath = Path.Combine(_webRootPath,"images",imgName);

        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }
        return $"/images/{imgName}";
    }
    public async Task<IEnumerable<string>> UploadImageAsync(IEnumerable<IFormFile> images)
    {
        if(_webRootPath is null)
        {
            throw new ArgumentNullException(nameof(_webRootPath));
        }
        var imageUrl = new List<string>();
        foreach (var image in images)
        {
            imageUrl.Add(await StoreImageToServer(image));
        }

        return imageUrl;
    }
}

