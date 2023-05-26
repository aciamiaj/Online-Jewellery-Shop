using System.ComponentModel.DataAnnotations;
namespace OnlineJewelleryShop.Models
{
    public class CartItem
    {
        [Required]
        [Key]
        public string cart_itemid { get; set; }
        [Required]
        public string CartId { get; set; }
        [Required]
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }

        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; }
    }
}
