using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebMaterial.DAL.Models
{
    public class Role
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Authority> Authorities { get; set; } = new List<Authority>();

        //public List<User> Users { get; set; } = new List<User>();
        public List<UserRole> UserRoles { get; set; }
        public List<RoleAuthority> RoleAuthorities { get; set; }
        public Role()
        {
            RoleAuthorities = new List<RoleAuthority>();
            UserRoles = new List<UserRole>();

        }
    }
}
