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
        private readonly IService<User> _iService;

        public UserController(IService<User> userService)
        {
            _iService = userService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            var userList = _iService.GetAll().ToList();

            if (userList.Count == 0)
            {
                return NoContent();
            }
            return Ok(userList);
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            var result = _iService.Get(id);
            
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public ActionResult<User> Delete(int id)
        {
            var result = _iService.Delete(id);

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
                var result = _iService.Add(obj);

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
                var result = _iService.Update(obj);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
