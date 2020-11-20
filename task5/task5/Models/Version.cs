using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace task5
{
    public class Version
    {
        [Key]
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public Material Material { get; set; }
        public DateTime UploadDateTime { get; set; }
        public long Size { get; set; }
        public int Release { get; set; }
        public string Path { get; set; }
    }
}
