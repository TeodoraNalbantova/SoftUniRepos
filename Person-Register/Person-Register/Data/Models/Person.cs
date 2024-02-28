using Person_Register.Data.Models;
using PersonRegister.Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonRegister.Data.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? City { get; set; }
        public int Age { get; set; }

        public int? RegionId { get; set; }

        public Region Region { get; set; }

    }
}
