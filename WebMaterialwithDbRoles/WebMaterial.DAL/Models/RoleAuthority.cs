using System;
using System.Collections.Generic;
using System.Text;

namespace WebMaterial.DAL.Models
{
    public class RoleAuthority
    { 
        public string RoleId { get; set; }
        public Role Role { get; set; }

        public string AuthorityId { get; set; }
        public Authority Authority { get; set; }
    }
}
