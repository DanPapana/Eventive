using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using System;
using System.IO;

namespace Eventive.ApplicationLogic.Services
{
    public class BaseService
    {
        public string CompressImage(IFormFile inputImage)
        {
            using var memoryStream = new MemoryStream();

            using (var image = Image.Load(inputImage.OpenReadStream()))
            {
                int width = 400;
                IResampler sampler = KnownResamplers.Lanczos3;
                bool compand = true;
                ResizeMode mode = ResizeMode.Crop;

                var resizeOptions = new ResizeOptions
                {
                    Size = new Size(width),
                    Sampler = sampler,
                    Compand = compand,
                    Mode = mode
                };

                image.Mutate(x => x
                     .Resize(resizeOptions));

                var afterMutations = image.Size();

                var encoder = new JpegEncoder()
                {
                    Quality = 100 //Variable between 1-100
                };

                image.Save(memoryStream, encoder);
                memoryStream.Position = 0;
            }

            return Convert.ToBase64String(memoryStream.ToArray());
        }
    }
}
