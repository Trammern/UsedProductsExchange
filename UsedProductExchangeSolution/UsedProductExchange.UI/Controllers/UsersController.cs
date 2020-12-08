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
    public class UsersController : ControllerBase
    {
        private readonly IService<User> _userService;
        public UsersController(IService<User> userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            var userList = _userService.GetAll().ToList();
            
            if (userList.Count == 0)
            {
                return NoContent();
            }
            return Ok(userList);
        }

        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            var result = _userService.Get(id);
            
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        
        [HttpPost]
        public ActionResult<User> Post([FromBody] User user)
        {
            try
            {
                var result = _userService.Add(user);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] User user)
        {
            try
            {
                var result = _userService.Update(user);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<User> Delete(int id)
        {
            var result = _userService.Delete(id);
            
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }
    }
}
