using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using WebMaterial.DAL.Data;
using WebMaterial.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using Version = WebMaterial.DAL.Models.Version;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebMaterial.BLL
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext _context;
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(ApplicationContext context, IConfiguration config, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _config = config;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public User Register(User user, string password)
        {
            var result = _userManager.CreateAsync(user, password);
            if (result != null)
            {
                _signInManager.SignInAsync(user, false);
                var role = _context.Rolles.Include(p => p.RoleAuthorities).ThenInclude(p => p.Authority).FirstOrDefault(p => p.Name == "reader");
                user.UserRoles.Add(new UserRole { Role = role, User = user });
                //user.Roles.Add(role);
                //role.Users.Add(user);
                _context.SaveChanges();
                //await _userManager.AddToRoleAsync(user, "reader");
                return user;
            }
            else
            {
                return null;
            }
        }
        public async Task<User> LoginAsync(User user, string password)
        {
            var result = await
                    _signInManager.PasswordSignInAsync(user.Email, password, false, false);
            if (result.Succeeded)
            {
                //User nuser = await _context.Users.Include(p => p.UserRoles).ThenInclude(s => s.Role).FirstOrDefaultAsync(e => e.Email == user.Email);
                return user;
            }
            else
            {
                return null;
            }
        }
        public bool AccessCheck(string login, string methodName)
        {
            var user = _context.Users.Include(p => p.UserRoles).ThenInclude(s => s.Role).FirstOrDefault(e => e.Email == login);
            foreach (var userRole in user.UserRoles)
            {
                if (userRole.Role.Name == "admin")
                    return true;
                var role = _context.Rolles.Include(p => p.RoleAuthorities).ThenInclude(s => s.Authority).FirstOrDefault(e => e.Name == userRole.Role.Name);
                foreach (var authority in role.RoleAuthorities)
                {
                    if (authority.Authority.Name.ToLower() == methodName.ToLower())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public User EditRoles(string email, string role)
        {
            User user = _context.Users.Include(s => s.UserRoles).ThenInclude(p => p.Role).FirstOrDefault(p => p.Email == email);//_userManager.FindByEmailAsync(email);
            Role newRole = _context.Rolles.Include(s => s.RoleAuthorities).ThenInclude(s => s.Authority).FirstOrDefault(s => s.Name == role);
            if (user != null)
            {
                user.UserRoles.Add(new UserRole { User = user, Role = newRole });
                _context.SaveChanges();
                return user;
            }
            return null;
        }
        public async Task Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
        }
    }
}
