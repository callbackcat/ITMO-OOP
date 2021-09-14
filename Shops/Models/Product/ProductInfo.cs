using Shops.Tools;

namespace Shops.Models.Product
{
    public class ProductInfo
    {
        public ProductInfo(double price, uint count)
        {
            if (price <= 0)
                throw new ShopException("Product's price should be greater than zero");

            Price = price;
            Count = count;
        }

        public double Price { get; set; }
        public uint Count { get; set; }
    }
}