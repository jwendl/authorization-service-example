using System;

namespace PolicyManager.DataAccess.Models
{
    public class ThingAttribute
        : BaseDatabaseModel
    {
        public Guid ThingId { get; set; }

        public Thing Thing { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
