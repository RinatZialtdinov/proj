using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AuthSample.Models;
using Microsoft.AspNetCore.Authorization;

namespace AuthSample.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            // return Content(User.Identity.Name);

            ViewData["UserEmail"] = User.Identity.Name;

            return View();
        }
    }
}
