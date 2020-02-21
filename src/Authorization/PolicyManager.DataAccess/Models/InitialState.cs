using System.Collections.Generic;
using System.Security.Claims;

namespace PolicyManager.DataAccess.Models
{
    public class InitialState<TGroup>
    {
        public ClaimsPrincipal ClaimsPrincipal { get; set; }

        public string Identifier { get; set; }

        public IEnumerable<TGroup> Groups { get; set; }
    }
}
