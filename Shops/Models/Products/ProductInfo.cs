using Shops.Tools;

namespace Shops.Models.Products
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

        public double Price { get; private set; }
        public uint Count { get; internal set; }

        internal void ChangePrice(double newPrice)
        {
            if (newPrice <= 0)
                throw new ShopException("Invalid new product's price");

            Price = newPrice;
        }
    }
}