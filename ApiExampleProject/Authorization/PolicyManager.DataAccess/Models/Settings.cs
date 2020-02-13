using System;

namespace PolicyManager.DataAccess.Models
{
    public class Settings
    {
        public string TenantId { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Audience { get; set; }

        public string Issuer { get; set; }

        public Uri DataUri { get; set; }

        public string DataUserName { get; set; }

        public string DataPassword { get; set; }
    }
}
