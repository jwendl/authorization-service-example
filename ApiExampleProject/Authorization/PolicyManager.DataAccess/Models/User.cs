using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PolicyManager.DataAccess.Models
{
    public class User
        : BaseDatabaseModel
    {
        public string UserPrincipalName { get; set; }

        [JsonIgnore]
        public ICollection<UserAttribute> UserAttributes { get; } = new List<UserAttribute>();
    }
}
