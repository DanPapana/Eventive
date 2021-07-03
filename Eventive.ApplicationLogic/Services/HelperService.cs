using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Eventive.ApplicationLogic.Services
{
    public static class HelperService
    {
        public static string CompressImage(IFormFile inputImage)
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
                    Size = new Size(width), Sampler = sampler, Compand = compand, Mode = mode
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

        public static async Task<string> GetCityFromAddress(string address)
        {
            string apiKey = ConfigurationManager.AppSettings.Get("GOOGLE_API_KEY");
            string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?key={1}&address={0}&sensor=false", 
                Uri.EscapeDataString(address), apiKey);

            using (var client = new HttpClient())
            {
                var request = await client.GetAsync(requestUri);
                var content = await request.Content.ReadAsStringAsync();
                var xmlDocument = XDocument.Parse(content);

                if (request.IsSuccessStatusCode)
                {
                    XElement result = xmlDocument.Element("GeocodeResponse").Element("result");
                    if (result != null)
                    {
                        var addressElements = result.Elements("address_component");

                        foreach (var add in addressElements)
                        {
                            if (add.Element("type").Value.Equals("locality"))
                            {
                                return add.Element("long_name").Value;
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
