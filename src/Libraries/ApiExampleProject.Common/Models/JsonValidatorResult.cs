using System.Net.Http;

namespace ApiExampleProject.Common.Models
{
    public class JsonValidationResult<TModel>
    {
        public TModel Item { get; set; }

        public bool IsValid { get; set; }

        public HttpResponseMessage Message { get; set; }
    }
}
