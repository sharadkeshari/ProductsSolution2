using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Item
    {
        public int Id { get; set; }  // Primary Key
        public int ProductId { get; set; }  // Foreign Key referencing Product
        public int Quantity { get; set; }  // Quantity in Stock
        public Product Product { get; set; }  // Navigation Property
    }
}

