using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Models;
using Shops.Models.Products;
using Shops.Tools;

namespace Shops.Services
{
    public class ShopManager : IShopManager
    {
        private readonly List<Shop> _shops;
        private readonly HashSet<Product> _productsRegister;

        public ShopManager()
        {
            _shops = new List<Shop>();
            _productsRegister = new HashSet<Product>();
        }

        public Shop Create(string name, string address)
        {
            var shop = new Shop(name, address);
            _shops.Add(shop);
            return shop;
        }

        public Product RegisterProduct(string name)
        {
            var product = new Product(name);
            _productsRegister.Add(product);
            return product;
        }

        public Shop FindShopWithLowestPrice(Guid id, uint count)
        {
            double lowestPrice = double.MaxValue;
            Shop cheapestShop = null;
            ProductInfo info = null;

            foreach (Shop shop in _shops)
            {
                try
                {
                    info = shop.GetProductInfo(id).Value;
                }
                catch (ShopException)
                {
                    continue;
                }

                if (info.Price < lowestPrice && info.Count >= count)
                {
                        lowestPrice = info.Price;
                        cheapestShop = shop;
                }
            }

            if (info is null)
            {
                throw new ShopException("None of the stores " +
                                        $"contain enough product with id: {id}");
            }

            return cheapestShop;
        }
    }
}