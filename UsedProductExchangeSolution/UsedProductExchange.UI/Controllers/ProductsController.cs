using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UsedProductExchange.Core.Application;
using UsedProductExchange.Core.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UsedProductExchange.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/<ProductsController>
        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            var productList = _productService.GetAllProduct().ToList();
            if (productList.Count == 0)
            {
                return NoContent();
            }
            return Ok(productList);
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public ActionResult<Product> Get(int id)
        {
            var result = _productService.GetProductById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // POST api/<ProductsController>
        [HttpPost]
        public ActionResult<Product> Post([FromBody] Product product)
        {
            try
            {
                var result = _productService.CreateProduct(product);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public ActionResult<Product> Put(int id, [FromBody] Product product)
        {
            try
            {
                var result = _productService.UpdateProduct(product);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public ActionResult<Product> Delete(int id)
        {
            var productToDelete = _productService.GetProductById(id);
            var result = _productService.DeleteProduct(productToDelete);
            if (result == null)
            {
                return NotFound();
            }
            return result;


        }
    }
}
