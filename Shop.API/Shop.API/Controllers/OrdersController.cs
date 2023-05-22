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
    public class OrdersController : ControllerBase
    {
        /*private readonly ShopContext _context;*/
        private readonly IUnitOfWork unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders =  await unitOfWork.OrderRepository.GetAllAsync();
            
            return orders.ToList();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await unitOfWork.OrderRepository.GetByIDAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            //_context.Entry(order).State = EntityState.Modified;
            unitOfWork.OrderRepository.Update(order);
            try
            {
                await unitOfWork.SaveAsync();
                //await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            /*_context.Orders.Add(order);
            await _context.SaveChangesAsync();*/
            unitOfWork.OrderRepository.Insert(order);
            await unitOfWork.SaveAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await unitOfWork.OrderRepository.GetByIDAsync(id);
                //await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            /* _context.Orders.Remove(order);
             await _context.SaveChangesAsync();*/
            unitOfWork.OrderRepository.Delete(id);
            await unitOfWork.SaveAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return unitOfWork.OrderRepository.Get(e => e.Id == id).Any();
        }
    }
}
