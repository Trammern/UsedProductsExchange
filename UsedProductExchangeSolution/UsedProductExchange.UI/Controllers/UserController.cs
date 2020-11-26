using System.Collections.Generic;
using UsedProductExchange.Core.Application;
using UsedProductExchange.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;

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
        public ActionResult<IEnumerable<User>> Get()
        {
            var userList = _iUserService.GetAllUsers().ToList();
            if (userList.Count == 0)
            {
                return NoContent();
            }
            return Ok(userList);
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            var result = _iUserService.GetUserById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public ActionResult<User> Delete(int id)
        {
            var result = _iUserService.DeleteUser(id);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }
        [HttpPost]
        public ActionResult<User> Post([FromBody] User obj)
        {
            try
            {
                var result = _iUserService.CreateUser(obj);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] User obj)
        {
            try
            {
                var result = _iUserService.UpdateUser(obj);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
