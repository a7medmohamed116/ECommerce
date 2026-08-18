using ECommerce.Application.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Image
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;

        public ImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveImageAsync(
            IFormFile image,
            string folderName)
        {
            var extension = Path.GetExtension(image.FileName).ToLower();

            var allowedExtensions = new[]
            {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };

            if (!allowedExtensions.Contains(extension))
                throw new InvalidOperationException("Invalid image extension.");

            var fileName = $"{Guid.NewGuid()}{extension}";

            var folderPath = Path.Combine(
                _environment.WebRootPath,
                "Files",
                "images",
                folderName);

            Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            await using var stream = new FileStream(
                filePath,
                FileMode.Create);

            await image.CopyToAsync(stream);

            return $"images/{folderName}/{fileName}";
        }
    }
}
