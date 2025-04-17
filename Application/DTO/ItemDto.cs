using System;

namespace Application.DTO
{
    public class ItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public string? ProductName { get; set; }  // Assuming you want to include the product name
    }
}
