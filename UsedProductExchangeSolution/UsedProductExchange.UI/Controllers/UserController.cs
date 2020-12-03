using System.Collections.Generic;
using UsedProductExchange.Core.Application;
using UsedProductExchange.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using UsedProductExchange.Core.Filter;

namespace UsedProductExchange.UI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IService<User> _userService;
        public UserController(IService<User> userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<FilteredList<Category>> Get([FromQuery] Filter filter)
        {
            try
            {
                return Ok(_userService.GetAll(filter));
            }
            catch (NullReferenceException e)
            {
                return StatusCode(404, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            var result = _userService.Get(id);
            
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
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
    }
}
