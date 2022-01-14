using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Services
{
    public class BasicImageService : IImageService
    {
        public string ContentType(IFormFile file)
        {
            return file?.ContentType; // ? returns null if no content type
        }

        public string DecodeImage(byte[] data, string type)
        {
            // Validate data or type, see if null
            if (data is null || type is null) return null;

            // Return image converted to base64 string
            return $"data:image/{type};base64,{Convert.ToBase64String(data)}";
        }

        public async Task<byte[]> EncodeImageAsync(IFormFile file)
        {
            // Validate file, see if null
            if (file is null) return null;

            // Instantiate new MemoryStream()
            using var ms = new MemoryStream();
            // Copy file to new MS asynchronously
            await file.CopyToAsync(ms);
            // Return MS as an array
            return ms.ToArray();

        }

        public async Task<byte[]> EncodeImageAsync(string fileName)
        {
            // Store file's storage path
            var file = $"{Directory.GetCurrentDirectory()}/wwwroot/img/{fileName}";

            // Read file's bytes, return byte array
            return await File.ReadAllBytesAsync(file);
        }

        public int Size(IFormFile file)
        {
            // Return size of file, if it's present, via it's length converted to int32
            return Convert.ToInt32(file?.Length);
        }
    }
}
