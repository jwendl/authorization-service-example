using System.IO;
using System.Threading.Tasks;

namespace ApiExampleProject.Common.Interfaces
{
    public interface IJsonTextSerializer
    {
        Task<T> DeserializeObjectAsync<T>(Stream contentStream);

        string SerializeObject<T>(T item);
    }
}
