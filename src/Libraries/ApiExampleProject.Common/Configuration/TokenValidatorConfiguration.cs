using System;
using System.Collections.Generic;

namespace ApiExampleProject.Common.Configuration
{
    public class TokenValidatorConfiguration
    {
        private const string DEFAULTAUTHORITY = "https://login.microsoftonline.com/{TenantId}/v2.0";
        private Uri authorityUri;
        private IEnumerable<string> validIssuers;
        private IEnumerable<string> validAudiences;

        public string TenantId { get; set; }

        public string Audience { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Authority { get; set; } = DEFAULTAUTHORITY;

        public Uri AuthorityUri
        {
            get
            {
                if (authorityUri == null)
                {
                    authorityUri = new Uri(Authority.Replace("{TenantId}", TenantId, StringComparison.OrdinalIgnoreCase));
                }

                return authorityUri;
            }
        }

        public IEnumerable<string> ValidIssuers
        {
            get
            {
                if (validIssuers == null)
                {
                    validIssuers = new string[]
                    {
                        $"https://login.microsoftonline.com/{TenantId}/",
                        $"https://login.microsoftonline.com/{TenantId}/v2.0",
                        $"https://login.windows.net/{TenantId}/",
                        $"https://login.microsoft.com/{TenantId}/",
                        $"https://sts.windows.net/{TenantId}/"
                    };
                }

                return validIssuers;
            }
        }

        public IEnumerable<string> ValidAudiences
        {
            get
            {
                if (validAudiences == null)
                {
                    validAudiences = new string[]
                    {
                        Audience,
                        ClientId
                    };
                }

                return validAudiences;
            }
        }
    }
}
