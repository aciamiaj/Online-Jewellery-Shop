using System.ComponentModel.DataAnnotations;

namespace OnlineJewelleryShop.Models
{
    public partial class ProductType
    {
        public ProductType()
        {
            Products = new HashSet<Product>();
        }

        [Required]
        //[StringLength(4)]
        public string ProdTypeId { get; set; } = null!;
        //[Required]
        public string? ProdTypeName { get; set; }
        public string? ProdTypeDescription { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
