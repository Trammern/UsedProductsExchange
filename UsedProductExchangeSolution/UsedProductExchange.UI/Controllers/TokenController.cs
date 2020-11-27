﻿using Microsoft.AspNetCore.Mvc;
using System.Linq;
using UsedProductExchange.Core.Application;
using UsedProductExchange.Core.Entities;

namespace UsedProductExchange.UI.Controllers
{
    [Route("/api/token")]
    public class TokenController : Controller
    {
        private readonly IService<User> _userService;
        private readonly ILoginService _loginService;

        public TokenController(IService<User> userService, ILoginService loginService)
        {
            _userService = userService;
            _loginService = loginService;
        }

        [HttpPost]
        public IActionResult Login([FromBody]LoginInputModel model)
        {
            var user = _userService.GetAll().FirstOrDefault(u => u.Username == model.Username);

            // Check if the user is not null, and it exists.
            if (user == null) return Unauthorized();

            // Check that the password is correct. If not we un-authorize the request.
            if (!_loginService.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
                return Unauthorized();

            // The user exist and the password is correct
            // so we return the username and a token back.
            return Ok(new
            {
                username = user.Username,
                token = _loginService.GenerateToken(user)
            });
        }

    }
}