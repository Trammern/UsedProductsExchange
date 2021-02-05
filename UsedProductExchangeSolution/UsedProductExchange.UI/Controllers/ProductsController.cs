using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UsedProductExchange.Core.Application;
using UsedProductExchange.Core.Entities;
using UsedProductExchange.Core.Filter;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UsedProductExchange.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController: ControllerBase
    {

        private readonly IService<Product> _productService;

        public ProductsController(IService<Product> productService)
        {
            _productService = productService;
        }
        
        [HttpGet]
        public ActionResult<FilteredList<Category>> Get([FromQuery] Filter filter)
        {
            try
            {
                return Ok(_productService.GetAll(filter));
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


        // GET api/products/5
        [HttpGet("{id}")]
        public ActionResult<Product> Get(int id)
        {
            var result = _productService.Get(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // POST api/products
        [HttpPost]
        public ActionResult<Product> Post([FromBody] Product product)
        {
            try
            {
                var result = _productService.Add(product);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }

        // PUT api/products/5
        [HttpPut("{id}")]
        public ActionResult<Product> Put(int id, [FromBody] Product product)
        {
            try
            {
                var result = _productService.Update(product);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE api/products/5
        [HttpDelete("{id}")]
        public ActionResult<Product> Delete(int id)
        {
            var result = _productService.Delete(id);
            
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }
    }
}
