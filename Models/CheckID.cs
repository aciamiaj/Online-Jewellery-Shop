using OnlineJewelleryShop.Models;

namespace OnlineJewelleryShop.Models
{
    public static class CheckID
    {

        public static string IDExists(JjewelleryContext ctx, string UserId)
        {
            string msg = "";
            if (!string.IsNullOrEmpty(UserId))
            {
                var user = ctx.Users.FirstOrDefault(
                    s => s.UserId.ToLower() == UserId.ToLower());
                if (user != null)
                    msg = $"UserId {UserId} already in use.";
            }
            return msg;
        }

    }
}
