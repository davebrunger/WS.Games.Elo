using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace WS.Games.Elo.Lib.Services
{
    public class ImageService
    {
        public async Task<byte[]> GetImageBytesAsync(string url)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            using (var response = await client.SendAsync(request))
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
        }

        public byte[] ResizeImage(byte[] bytes, int maxSize)
        {
            using(var inputStream = new MemoryStream(bytes))
            {
                var image = Image.FromStream(inputStream);
                var currentSize = Math.Max(image.Size.Width, image.Size.Height);
                if (currentSize <= maxSize)
                {
                    return bytes;
                }

                var newSize = new Size(
                    (image.Size.Width * maxSize) / currentSize,
                    (image.Size.Height * maxSize) / currentSize); 

                var resizedImage = new Bitmap(image, newSize);
                using(var outputStream = new MemoryStream())
                {
                    resizedImage.Save(outputStream, ImageFormat.Png);
                    return outputStream.ToArray();
                }
            }
        }
    }
}