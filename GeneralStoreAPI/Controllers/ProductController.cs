﻿using GeneralStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GeneralStoreAPI.Controllers
{
    public class ProductController : ApiController
    {
        private StoreDbContext _context = new StoreDbContext();

        private string GenerateSku(string productName)
        {
            // Initialize a Random object so we can randomly assign this a number
            Random random = new Random();
            // Get a Random number and turn it into a string
            var randItemNum = random.Next(0, 1000).ToString();
            // Construct a 3 character string based on the above number. If the number is less than 3 digits, add 0's in front of it to make it 3 digits
            var itemId = new string('0', 3 - randItemNum.Length) + randItemNum;
            // Create the entire SKU and return it
            return $"EFA-{productName.Substring(0, 3)}-{itemId}";
        }

        public IHttpActionResult Post(Product product)
        {
            if (product == null)
            {
                return BadRequest("Your request body cannot be empty.");
            }

            product.SKU = GenerateSku(product.Name);

            if (ModelState.IsValid && product.SKU != null)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return Ok();
            }

            return BadRequest(ModelState);
        }

        public IHttpActionResult Get()
        {
            List<Product> products = _context.Products.ToList();
            if (products.Count != 0)
            {
                return Ok(products);
            }

            return BadRequest("Your database contains no Products.");
        }

        // Get by ID
        public IHttpActionResult Get(string sku)
        {
            Product product = _context.Products.Find(sku);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        public IHttpActionResult Put(string sku, Product updatedProduct)
        {
            if (ModelState.IsValid)
            {
                Product product = _context.Products.Find(sku);

                if (product != null)
                {
                    product.Name = updatedProduct.Name;
                    product.Cost = updatedProduct.Cost;
                    product.NumberInStock = updatedProduct.NumberInStock;

                    _context.SaveChanges();

                    return Ok("Product has been updated.");
                }

                return NotFound();
            }
            return BadRequest(ModelState);
        }

        public IHttpActionResult Delete(string sku)
        {
            Product entity = _context.Products.Find(sku);

            if (entity == null)
            {
                return NotFound();
            }

            _context.Products.Remove(entity);

            if (_context.SaveChanges() == 1)
            {
                return Ok("The product was deleted.");
            }

            return InternalServerError();
        }
    }
}
