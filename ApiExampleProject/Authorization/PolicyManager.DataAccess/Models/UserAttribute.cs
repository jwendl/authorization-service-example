using System;

namespace PolicyManager.DataAccess.Models
{
    public class UserAttribute
        : BaseDatabaseModel
    {
        public Guid UserId { get; set; }

        public User User { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
