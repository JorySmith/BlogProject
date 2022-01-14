using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Services
{
    public interface IImageService // Interface methods to be implemented by concrete classes
    {
        // Take in IFormFile image, encode it, return byte array for DB storage
        Task<byte[]> EncodeImageAsync(IFormFile file);

        // Encode images already stored in a path
        Task<byte[]> EncodeImageAsync(string fileName);

        // Decode images to display them
        string DecodeImage(byte[] data, string type);

        // Image content type
        string ContentType(IFormFile file);

        //  Image size
        int Size(IFormFile file);
    }
}
