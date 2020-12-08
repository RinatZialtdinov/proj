using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebMaterial.DAL.Models
{
    public class Authority
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        //public List<Role> Roles { get; set; } = new List<Role>();
        public List<RoleAuthority> RoleAuthorities { get; set; }

        public Authority()
        {
            RoleAuthorities = new List<RoleAuthority>();
        }
    }
}
