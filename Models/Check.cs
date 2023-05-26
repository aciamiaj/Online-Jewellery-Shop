using Microsoft.CodeAnalysis;

namespace OnlineJewelleryShop.Models
{
    // This class contains a static method to check if a given category ID already exists in the database.
    public class Check
    {
        public static string IDExists(JjewelleryContext ctx,
            string categoryid)
        {
            string msg = "";
            if (!string.IsNullOrEmpty(categoryid))
            {
                // Check if the category ID already exists in the Products table
                var product = ctx.Products.FirstOrDefault(
                    s => s.ProductId.ToLower() == categoryid.ToLower());
                if (product != null)
                    msg = $"Category ID {categoryid} already in use.";
            }
            return msg;
        }
    }
}
