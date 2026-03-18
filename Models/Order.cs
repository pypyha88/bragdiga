using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace praktika.Models
{
    public class Order
    {
        [Key]
        public int IdOrder { get; set; }

        public int IdUser { get; set; }
        [ForeignKey("IdUser")]
        public User? User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Новый";

        public decimal TotalPrice { get; set; }

        public List<OrderItem> Items { get; set; } = new();
    }

    public class OrderItem
    {
        [Key]
        public int IdOrderItem { get; set; }

        public int IdOrder { get; set; }
        [ForeignKey("IdOrder")]
        public Order? Order { get; set; }

        public int IdProduct { get; set; }
        [ForeignKey("IdProduct")]
        public Product? Product { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}