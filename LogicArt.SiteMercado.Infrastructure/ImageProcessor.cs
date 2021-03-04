using System.IO;
using System.Threading.Tasks;
using LogicArt.SiteMercado.Application;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace LogicArt.SiteMercado.Infrastructure
{
    public class ImageProcessor : IImageProcessor
    {

        public async Task<byte[]> ProcessImage(byte[] imageBytes)
        {
            using var image = Image.Load(imageBytes);
            var resizeOptions = new ResizeOptions { Size = new Size(1280, 720), Mode = ResizeMode.Min };
            image.Mutate(context => context.Resize(resizeOptions));
            var encoder = new JpegEncoder { Quality = 80 };
            await using var stream = new MemoryStream();
            await image.SaveAsync(stream, encoder);
            return stream.ToArray();
        }

    }
}
