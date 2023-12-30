using Application.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services.ImagesUploaders;

public class ImageExtensionValidator : IImageExtensionValidator
{
    private HashSet<string> _imageExtensions;
    public ImageExtensionValidator(IConfiguration config)
    {
        var extensionConfig = config.GetSection("ValidImageExtensions") ?? throw new ArgumentNullException("Image extensions.");
        _imageExtensions = 
            extensionConfig
            .AsEnumerable()
            .Where(c=> c.Value != null)
            .Select(c=> c.Value)
            .ToHashSet()!;
    }
    public bool Validate(string extension)
    {
        return _imageExtensions.Contains(extension);
    }
}

