using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using ApiExampleProject.Common.Interfaces;

namespace ApiExampleProject.Common.Serializers
{
    public class JsonTextSerializer
        : IJsonTextSerializer
    {
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public JsonTextSerializer()
        {
            jsonSerializerOptions = new JsonSerializerOptions();
        }

        public async Task<T> DeserializeObjectAsync<T>(Stream contentStream)
        {
            return await JsonSerializer.DeserializeAsync<T>(contentStream, jsonSerializerOptions);
        }

        public string SerializeObject<T>(T item)
        {
            return JsonSerializer.Serialize(item, jsonSerializerOptions);
        }
    }
}
