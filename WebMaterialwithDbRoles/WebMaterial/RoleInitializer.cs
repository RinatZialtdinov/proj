using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaterial.DAL.Data;
using WebMaterial.DAL.Models;

namespace WebMaterial
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, ApplicationContext _context)
        {
            Role adminRole = new Role { Name = "admin", Id = "1" };
            Role readerRole = new Role { Name = "reader", Id = "2" };
            Role initiatorRole = new Role { Name = "initiator", Id = "3" };
            Authority editRole = new Authority { Name = "editRole", Id = "4" };
            Authority createEssence = new Authority { Name = "createEssence", Id = "5" };
            Authority readEssence = new Authority { Name = "readEssence", Id = "6" };
            string adminEmail = "admin@gmail.comm";
            string password = "Aa123456";

            if (_context.Rolles.FirstOrDefault(p => p.Name == "reader") == null)
            {
                _context.Rolles.AddRange(adminRole, readerRole, initiatorRole);
                _context.Authorities.AddRange(editRole, createEssence, readEssence);

                adminRole.RoleAuthorities.Add(new RoleAuthority { Role = adminRole, Authority = editRole });
                readerRole.RoleAuthorities.Add(new RoleAuthority { Role = readerRole, Authority = readEssence });
                initiatorRole.RoleAuthorities.Add(new RoleAuthority { Role = initiatorRole, Authority = createEssence });
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                admin.UserRoles.Add(new UserRole { Role = adminRole, User = admin });
            }
            _context.SaveChanges();
        }
    }
}
