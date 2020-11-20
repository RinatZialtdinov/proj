using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace task5
{
    public enum Categories
    {
        Presentation = 1,
        Application,
        Other
    }
    public class Material
    {
        [Key]
        public int Id { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public List<Version> Versions { get; set; } = new List<Version>();
    }
}
