using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaterial.DAL.Models;

namespace WebMaterial.DTO
{
    public class NewMaterialDto
    {
        public string Name { get; set; }
        public Category Category { get; set; }
    }
}
