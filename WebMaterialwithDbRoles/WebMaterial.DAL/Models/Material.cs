using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebMaterial.DAL.Models
{
    public enum Category
    {
        Presentation,
        Application,
        Other
    }
    public class Material
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
        public ICollection<Version> Versions { get; set; }
        public string Extensio { get; set; }
    }
}
