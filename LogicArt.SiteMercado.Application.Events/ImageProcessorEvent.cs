using System.Threading.Tasks;
using LogicArt.Arch.Application.Bus.Abstractions;
using LogicArt.SiteMercado.Domain.Entities;

namespace LogicArt.SiteMercado.Application.Events
{
    public class ImageProcessorEvent : IPreInsertEvent
    {

        private readonly IImageProcessor _imageProcessor;

        public ImageProcessorEvent(IImageProcessor imageProcessor) => _imageProcessor = imageProcessor;

        public async Task<bool> OnPreInsertAsync(object entity)
        {
            var product = (Product)entity;
            if (product.Image == null)
            {
                return false;
            }

            var imageBytes = await _imageProcessor.ProcessImage(product.Image);
            product.Image = imageBytes;

            return false;
        }

    }
}
