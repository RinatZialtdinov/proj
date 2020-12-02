using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace task5
{
    public class NewMaterialDto
    {
        //public IFormFile File { get; set; }
       // public byte[] File { get; set; }

        public string Name { get; set; }
        public string Category { get; set; }
    }
}
