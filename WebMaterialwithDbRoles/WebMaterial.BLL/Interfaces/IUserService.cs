﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebMaterial.DAL.Models;

namespace WebMaterial.BLL
{
    public interface IUserService
    {
        public User Register(User user, string password);
        public Task<User> LoginAsync(User user, string password);
        public bool AccessCheck(string login, string methodName);
        public User EditRoles(string email, string role);
        public Task Logout();
    }
}
