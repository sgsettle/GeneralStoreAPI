using GeneralStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GeneralStoreAPI.Controllers
{
    public class TransactionController : ApiController
    {
        private StoreDbContext _context = new StoreDbContext();

        public IHttpActionResult Post(Transaction transaction)
        {
            if (transaction == null)
                return BadRequest("Your request body cannot be empty.");

            if (ModelState.IsValid && transaction.CustomerId != 0 && transaction.ProductSKU != null)
            {
                _context.Transactions.Add(transaction);
                _context.SaveChanges();
                return Ok();
            }

            return BadRequest(ModelState);
        }

        public IHttpActionResult Get()
        {
            List<Transaction> transactions = _context.Transactions.ToList();
            if (transactions.Count != 0)
            {
                return Ok(transactions);
            }

            return BadRequest("Your database contains no Transactions.");
        }

        // Get by ID
        public IHttpActionResult Get(int id)
        {
            Transaction transaction = _context.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        public IHttpActionResult Put(int id, Transaction updatedTransaction)
        {
            if (ModelState.IsValid)
            {
                Transaction transaction = _context.Transactions.Find(id);

                if (transaction != null)
                {
                    transaction.ItemCount = updatedTransaction.ItemCount;
                    transaction.DateOfTransaction = updatedTransaction.DateOfTransaction;

                    _context.SaveChanges();

                    return Ok("Transaction has been updated.");
                }

                return NotFound();
            }
            return BadRequest(ModelState);
        }

        public IHttpActionResult Delete(int id)
        {
            Transaction entity = _context.Transactions.Find(id);

            if (entity == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(entity);

            if (_context.SaveChanges() == 1)
            {
                return Ok("The transaction was deleted.");
            }

            return InternalServerError();
        }
    }
}
