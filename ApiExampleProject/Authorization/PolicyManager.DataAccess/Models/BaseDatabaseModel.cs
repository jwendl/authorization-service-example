using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolicyManager.DataAccess.Models
{
    public class BaseDatabaseModel
    {
        public Guid Id { get; set; }
    }
}
