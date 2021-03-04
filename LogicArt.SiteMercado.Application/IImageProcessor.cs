using System.Threading.Tasks;

namespace LogicArt.SiteMercado.Application
{
    public interface IImageProcessor
    {

        Task<byte[]> ProcessImage(byte[] imageBytes);

    }
}
