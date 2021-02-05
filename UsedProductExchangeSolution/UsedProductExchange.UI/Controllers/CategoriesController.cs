using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsedProductExchange.Core.Application;
using UsedProductExchange.Core.Entities;
using UsedProductExchange.Core.Filter;

namespace UsedProductExchange.UI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly IService<Category> _service;

        public CategoriesController(IService<Category> service)
        {
            _service = service; 
        }
        
        [HttpGet]
        public ActionResult<FilteredList<Category>> Get([FromQuery] Filter filter)
        {
            try
            {
                return Ok(_service.GetAll(filter));
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
        
        // GET api/categories/5 -- READ By Id
        [HttpGet("{id}")]
        public ActionResult<Category> Get(int id)
        {
            if (id < 1) return BadRequest("Id must be greater then 0");
            
            var entity = _service.Get(id);

            if (entity == null) return NotFound();
            
            return Ok(entity);
        }
        
        // POST api/categories -- CREATE
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult<Category> Post([FromBody] Category entity)
        {
            try
            {
                var entityFromDb = _service.Add(entity);
                return Created($"api/categories/{entityFromDb.CategoryId}", entityFromDb);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        // PUT api/categories/5 -- Update
        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        public ActionResult<Category> Put(int id, [FromBody] Category entity)
        {
            if (id < 1) return BadRequest("Id must be greater then 0");
            
            if (id != entity.CategoryId)
            {
                return BadRequest("Parameter Id and category ID must be the same");
            }
            
            if (_service.Get(id) == null) return NotFound();

            return Accepted(_service.Update(entity));
        }
        
        // DELETE api/categories/5
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public ActionResult<Category> Delete(int id)
        {
            if (id < 1) return BadRequest("Id must be greater then 0");
            
            if (_service.Get(id) == null) return NotFound();
            
            var category = _service.Delete(id);
            
            return Accepted(
                new
                {
                    message = "Object has been deleted",
                    category
                }
            );
        }
    }
}