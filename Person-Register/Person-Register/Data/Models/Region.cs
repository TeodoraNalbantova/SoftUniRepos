using PersonRegister.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Person_Register.Data.Models
{
    public class Region
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Person> Persons { get; set; }
    }
}
