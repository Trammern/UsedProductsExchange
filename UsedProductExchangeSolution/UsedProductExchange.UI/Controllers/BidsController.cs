using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsedProductExchange.Core.Application;
using UsedProductExchange.Core.Entities;
using UsedProductExchange.Core.Filter;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UsedProductExchange.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidsController : ControllerBase
    {
        private readonly IService<Bid> _service;
        private readonly IService<Product> _productService;

        public BidsController(IService<Bid> service, IService<Product> productService)
        {
            _service = service;
            _productService = productService;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public ActionResult<IEnumerable<Bid>> GetAll()
        {
            try
            {
                var bidsList = _service.GetAll().ToList();

                if (bidsList.Count == 0)
                {
                    return NoContent();
                }
                return Ok(bidsList);
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public ActionResult<Bid> Get(int id)
        {
            try
            {
                if (id < 1)
                {
                    return BadRequest("Id must be greater then 0");
                }

                var entity = _service.Get(id);

                if (entity == null)
                {
                    return NotFound();
                }
                return Ok(entity);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // POST api/<ValuesController>
        [HttpPost]
        public ActionResult<Bid> Post([FromBody] Bid entity)
        {
            try
            {
                var product = _productService.Get(entity.ProductId);
                entity.Product = product;
                var entityFromDb = _service.Add(entity);
                
                product.CurrentPrice = entityFromDb.Price;
                _productService.Update(product);
                
                return Ok(entityFromDb);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // DELETE api/<ValuesController>/5
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public ActionResult<Bid> Delete(int id)
        {
            try
            {
                if (id < 1) return BadRequest("Id must be greater then 0");

                if (_service.Get(id) == null) return NotFound();

                var bid = _service.Delete(id);

                return Accepted(
                    new
                    {
                        message = "Bid was been removed ",
                        bid
                    }
                );
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
