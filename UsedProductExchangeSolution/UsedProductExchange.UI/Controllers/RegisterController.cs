using Microsoft.AspNetCore.Mvc;
using System;
using UsedProductExchange.Core.Application;
using UsedProductExchange.Core.Entities;

namespace UsedProductExchange.UI.Controllers
{
    [Route("/api/register")]
    public class RegisterController : Controller
    {
        private readonly IService<User> _userService;
        private readonly ILoginService _loginService;

        public RegisterController(IService<User> userService, ILoginService loginService)
        {
            _userService = userService;
            _loginService = loginService;
        }

        [HttpPost]
        public IActionResult Register([FromBody]RegisterInputModel model)
        {
            try
            {
                _loginService.CreatePasswordHash(model.Password, out var passHash, out var passSalt);

                var user = _userService.Add(new User
                {
                    Name = model.Name,
                    Username = model.Username,
                    PasswordHash = passHash,
                    PasswordSalt = passSalt,
                    IsAdmin = model.IsAdmin,
                    Address = model.Address,
                    Email = model.Email,
                });

                return Ok(new
                {
                    //username = user.Username,
                    account = new
                    {
                        user.UserId,
                        user.Name,
                        user.Username,
                        user.IsAdmin,
                        user.Address,
                        user.Email
                    },
                    token = _loginService.GenerateToken(user)
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

    }
}
