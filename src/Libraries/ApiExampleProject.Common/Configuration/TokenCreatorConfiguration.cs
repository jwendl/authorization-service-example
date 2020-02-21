using System;

namespace ApiExampleProject.Common.Configuration
{
    public class TokenCreatorConfiguration
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Scopes { get; set; }

        public Guid TenantId { get; set; }
    }
}
