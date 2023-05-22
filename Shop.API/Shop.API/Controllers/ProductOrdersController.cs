using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.DAL;
using Shop.DAL.Data;
using Shop.DAL.models;

namespace Shop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductOrdersController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public ProductOrdersController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/ProductOrders
        [HttpGet]
        public ActionResult<IEnumerable<ProductOrder>> GetProductOrders()
        {
            var repos =  unitOfWork.ProductOrderRepository.Get(includes: "Product,Order");
            return repos.ToList();
        }

        // GET: api/ProductOrders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductOrder>> GetProductOrder(int id)
        {
            var productOrder = await unitOfWork.ProductOrderRepository.GetByIDAsync(id);

            if (productOrder == null)
            {
                return NotFound();
            }

            return productOrder;
        }

        // PUT: api/ProductOrders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductOrder(int id, ProductOrder productOrder)
        {
            if (id != productOrder.Id)
            {
                return BadRequest();
            }

            unitOfWork.ProductOrderRepository.Update(productOrder);
            try
            {
                await unitOfWork.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductOrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProductOrders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductOrder>> PostProductOrder(ProductOrder productOrder)
        {
            unitOfWork.ProductOrderRepository.Insert(productOrder);
            //_context.ProductOrders.Add(productOrder);
            await unitOfWork.SaveAsync();

            return CreatedAtAction("GetProductOrder", new { id = productOrder.Id }, productOrder);
        }

        // DELETE: api/ProductOrders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductOrder(int id)
        {
            var productOrder = await unitOfWork.ProductOrderRepository.GetByIDAsync(id);
            if (productOrder == null)
            {
                return NotFound();
            }

            unitOfWork.ProductOrderRepository.Delete(id);
            await unitOfWork.SaveAsync();

            return NoContent();
        }

        private bool ProductOrderExists(int id)
        {
            return unitOfWork.ProductOrderRepository.Get(e => e.Id == id).Any();
        }
    }
}
