using System.Collections.Generic;
using UsedProductExchange.Core.Application;
using UsedProductExchange.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace UsedProductExchange.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _iUserService;
        public UserController(IUserService userService)
        {
            _iUserService = userService;
        }

        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _iUserService.GetAllUsers();
        }
    }
}
