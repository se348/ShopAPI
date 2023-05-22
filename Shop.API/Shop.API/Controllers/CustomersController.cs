using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class CustomersController : ControllerBase
    {
       /* private readonly ShopContext _context;*/
       private readonly IUnitOfWork unitOfWork;

        public CustomersController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var customers = await unitOfWork.CustomerRepository.GetAsync(includes: i => i.Orders);
            return customers.ToList();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var temp = await unitOfWork.CustomerRepository.GetAsync(includes: i => i.Orders, filter: y => y.Id == id);
            var customer = temp.FirstOrDefault();
            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            unitOfWork.CustomerRepository.Update(customer);
            try
            {
                await unitOfWork.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {

            unitOfWork.CustomerRepository.Insert(customer);
            await unitOfWork.SaveAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await unitOfWork.CustomerRepository.GetByIDAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            unitOfWork.CustomerRepository.Delete(id);
            await unitOfWork.SaveAsync();   
            
            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return unitOfWork.CustomerRepository.Get(e => e.Id == id).Any(); 
        }
    }
}
