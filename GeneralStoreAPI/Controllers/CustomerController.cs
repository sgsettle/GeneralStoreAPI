using GeneralStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GeneralStoreAPI.Controllers
{
    public class CustomerController : ApiController
    {
        private StoreDbContext _context = new StoreDbContext();

        public IHttpActionResult Post(Customer customer)
        {
            if (customer == null)
                return BadRequest("Your request body cannot be empty.");

            if (ModelState.IsValid)
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();
                return Ok();
            }

            return BadRequest(ModelState);
        }

        // Get All
        public IHttpActionResult Get()
        {
            List<Customer> customers = _context.Customers.ToList();
            if (customers.Count != 0)
            {
                return Ok(customers);
            }

            return BadRequest("Your database contains no Customers.");
        }

        // Get by ID
        public IHttpActionResult Get(int id)
        {
            Customer customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        public IHttpActionResult Put(int id, Customer updatedCustomer)
        {
            if (ModelState.IsValid)
            {
                Customer customer = _context.Customers.Find(id);

                if (customer != null)
                {
                    customer.FirstName = updatedCustomer.FirstName;
                    customer.LastName = updatedCustomer.LastName;

                    _context.SaveChanges();

                    return Ok("Customer has been updated.");
                }

                return NotFound();
            }
            return BadRequest(ModelState);
        }

        public IHttpActionResult Delete(int id)
        {
            Customer entity = _context.Customers.Find(id);

            if (entity == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(entity);

            if (_context.SaveChanges() == 1)
            {
                return Ok("The customer was deleted.");
            }

            return InternalServerError();
        }
    }
}
