using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace WebMaterial.DAL.Models
{
    public class User : IdentityUser
    {
        //[Key]
        //IdentityUserLogin<string> Email { get; set; }
        //public int Id { get; set; }
        public string FirstName { get; set; }
        //public List<Role> Roles { get; set; } = new List<Role>();
        public List<UserRole> UserRoles { get; set; }

        public User()
        {
            UserRoles = new List<UserRole>();
        }
    }
}
