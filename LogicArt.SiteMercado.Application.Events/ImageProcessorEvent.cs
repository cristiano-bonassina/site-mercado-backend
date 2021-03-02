using System.IO;
using System.Threading.Tasks;
using LogicArt.Arch.Application.Bus.Abstractions;
using LogicArt.SiteMercado.Domain.Entities;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace LogicArt.SiteMercado.Application.Events
{
    public class ImageProcessorEvent : IPreInsertEvent
    {
        public async Task<bool> OnPreInsertAsync(object entity)
        {

            var product = (Product)entity;
            if (product.Image == null)
            {
                return false;
            }

            using var image = Image.Load(product.Image);
            var resizeOptions = new ResizeOptions
            {
                Size = new Size(1280, 720),
                Mode = ResizeMode.Min
            };
            image.Mutate(x => x.Resize(resizeOptions));
            var encoder = new JpegEncoder { Quality = 80 };
            await using var stream = new MemoryStream();
            await image.SaveAsync(stream, encoder);
            product.Image = stream.ToArray();

            return false;

        }

    }
}
