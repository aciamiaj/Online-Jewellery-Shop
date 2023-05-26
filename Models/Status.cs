using System.ComponentModel.DataAnnotations;

namespace OnlineJewelleryShop.Models
{
    public partial class Status
    {
        public Status()
        {
            ProductDetails = new HashSet<ProductDetail>();
        }

        [Required]
        //[StringLength(4)]
        public string StatusId { get; set; } = null!;
        //[Required]
        public string? StatusName { get; set; }

        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
    }
}
