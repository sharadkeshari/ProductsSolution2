using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }  // Primary Key
        public string ProductName { get; set; }  // Product Description
        public string CreatedBy { get; set; }  // Creator
        public DateTime CreatedOn { get; set; }  // Creation Timestamp
        public string? ModifiedBy { get; set; }  // Last Modifier (Nullable)
        public DateTime? ModifiedOn { get; set; }  // Modification Timestamp (Nullable)
    }
}

