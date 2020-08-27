using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GeneralStoreAPI.Models
{
    public class Product
    {
        [Key]
        public string SKU { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Cost { get; set; }
        [Required]
        public int NumberInStock { get; set; }
        public bool IsInStock
        {
            get
            {
                if (NumberInStock > 0)
                    return true;
                else
                    return false;
            }
        }
    }
}