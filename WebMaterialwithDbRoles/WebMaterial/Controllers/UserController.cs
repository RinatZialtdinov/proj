using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WebMaterial.DAL.Models;
using WebMaterial.DTO;
using Microsoft.AspNetCore.Authorization;
using WebMaterial.BLL;
using System.Diagnostics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebMaterial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/<ValuesController>
        [HttpGet]
       // [Authorize(Roles = "admin")]
        public ActionResult<IEnumerable<string>> Get() // для админа
        {
            string login = HttpContext.User.Identity.Name;
            StackTrace stackTrace = new StackTrace();
            string methodName = stackTrace.GetFrame(0).GetMethod().Name;
            if (_userService.AccessCheck(login, methodName))
                return Ok(new string[] { "value1", "value2" });
            return BadRequest();
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        //[Authorize(Roles = "reader")]
        public ActionResult<string> CreateEssence(int id) //для инициатора и админа
        {
            string login = HttpContext.User.Identity.Name;
            StackTrace stackTrace = new StackTrace();
            string methodName = stackTrace.GetFrame(0).GetMethod().Name;
            if (_userService.AccessCheck(login, methodName))
                return Ok("value");
            return BadRequest();
        }

        [HttpGet]
        [Route("init")]
        //[Authorize(Roles = "initiator")]
        public ActionResult<string> GetByInitiator() // для чтеца и админа
        {
            string login = HttpContext.User.Identity.Name;
            StackTrace stackTrace = new StackTrace();
            string methodName = stackTrace.GetFrame(0).GetMethod().Name;
            if (_userService.AccessCheck(login, methodName))
                return Ok("Вход от имени чтеца или админа");
            return BadRequest();
        }

        // POST api/<userController>
        [HttpPost]
        public ActionResult<User> Register([FromForm] UserDto newUser)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = newUser.Email, FirstName = newUser.Name, UserName = newUser.Email };
                var result = _userService.Register(user, newUser.Password);
                if (result != null)
                {
                    return Ok(user);
                }
            }
            return BadRequest();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route("login")]
        public ActionResult<User> Login([FromForm] UserDto model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { FirstName = model.Name, Email = model.Email };
                var result = _userService.LoginAsync(user, model.Password);
                if (result != null)
                    return Ok(result);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("edit")]
        //[Authorize(Roles = "admin")]
        public ActionResult EditRoles(string email, string role)
        {
            string login = HttpContext.User.Identity.Name;
            StackTrace stackTrace = new StackTrace();
            string methodName = stackTrace.GetFrame(0).GetMethod().Name;
            if (_userService.AccessCheck(login, methodName))
            {
                var result = _userService.EditRoles(email, role);
                if (result != null)
                    return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        [Route("logout")]
        public IActionResult Logout()
        {
            _userService.Logout();
            return Ok();
        }
    }
}
