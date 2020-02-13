using System;

namespace ApiExampleProject.CustomerData.DataAccess.Models
{
    public class Customer
        : BaseDatabaseModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
