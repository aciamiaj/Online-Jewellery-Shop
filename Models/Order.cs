using System.ComponentModel.DataAnnotations;

namespace OnlineJewelleryShop.Models
{
    public partial class Order
    {
        public Order()
        {
            ProductOrders = new HashSet<ProductOrder>();
        }

        [Required]
        //[StringLength(4)]
        public string OrderId { get; set; } = null!;
        //[Required]
        //[StringLength(4)]
        public string? UserId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Total { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? DeliveredDate { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
