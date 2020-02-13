using System.Collections.Generic;
using System.Linq;
using Group = Microsoft.Graph.Group;

namespace PolicyManager.DataAccess.Functions
{
    public static class ListParser
    {
        public static bool AnyGroup(dynamic dynamicGroups, string displayName)
        {
            var groups = dynamicGroups as List<Group>;
            return groups.Where(g => g.DisplayName == displayName).Any();
        }
    }
}
