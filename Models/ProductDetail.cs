using System.ComponentModel.DataAnnotations;

namespace OnlineJewelleryShop.Models
{
    public partial class ProductDetail
    {
        [Required]
        //[StringLength(4)]
        public string ProductDetailsId { get; set; } = null!;
        //[Required]
        //[StringLength(4)]
        public string? ProductId { get; set; }
        public string? Dimension { get; set; }
        //[Required]
        public int? AvailQty { get; set; }
        //[Required]
        //[StringLength(4)]
        public string? StatusId { get; set; }
        [Required]
        [Range(0.1, 1000000.0, ErrorMessage = "Price must be more than 0.")]
        public decimal? Price { get; set; }
        public string? Description { get; set; }

        public virtual Product? Product { get; set; }
        public virtual Status? Status { get; set; }
    }
}
