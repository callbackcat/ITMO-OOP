using System.Collections.Generic;
using Shops.Models.Products;
using Shops.Tools;

namespace Shops.Models
{
    public class ShoppingList
    {
        private readonly Dictionary<Product, int> _list;

        public ShoppingList()
        {
            _list = new Dictionary<Product, int>();
        }

        public void AddToList(Product product, int count)
        {
            if (product is null)
                throw new ShopException("The product is not valid");

            if (count <= 0)
                throw new ShopException("The product's count must be greater than zero");

            _list.Add(product, count);
        }

        public Dictionary<Product, int> GetList() => _list;
    }
}