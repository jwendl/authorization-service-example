using System.Diagnostics.CodeAnalysis;

namespace ApiExampleProject.Common.Constants
{
    public static class ContentTypes
    {
        [SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Keeping this for maintainability and ease of use.")]
        public static class Application
        {
            public const string Json = "application/json";
        }

        [SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Keeping this for maintainability and ease of use.")]
        public static class TextType
        {
            public const string Plain = "text/plain";

            public const string Html = "text/html";
        }
    }
}
