using System;

namespace PolicyManager.DataAccess.Models
{
    public class ThingPolicy
        : BaseDatabaseModel
    {
        public Guid ThingId { get; set; }

        public Thing Thing { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Expression { get; set; }
    }
}
