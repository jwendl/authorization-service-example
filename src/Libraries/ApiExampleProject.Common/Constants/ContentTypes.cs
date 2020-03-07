using System.Diagnostics.CodeAnalysis;

namespace ApiExampleProject.Common.Constants
{
    public static class ContentTypes
    {
        [SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "This is intentional to get ContentTypes.Application.Json")]
        public static class Application
        {
            public const string Json = "application/json";
        }

        [SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "This is intentional to get ContentTypes.TextType.Plain etc.")]
        public static class TextType
        {
            public const string Plain = "text/plain";

            public const string Html = "text/html";
        }
    }
}
