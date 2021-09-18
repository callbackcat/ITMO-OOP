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
            var product = new Product(name, Guid.NewGuid());
            _productsRegister.Add(product);
            return product;
        }

        public Shop FindShop(string name, string address)
        {
            return _shops.Find(s => s.Name == name
                                    && s.Address == address);
        }

        public Shop FindShopWithLowestPrice(ShoppingList list)
        {
            Dictionary<Product, int> products = list.GetList();
            double minSum = double.MaxValue;
            Shop cheapestShop = null;
            double totalSum = 0;

            foreach (Shop shop in _shops)
            {
                foreach (var product in products)
                {
                    ProductInfo info;
                    try
                    {
                        info = shop.GetProductInfo(product.Key.Id).Value;
                    }
                    catch (ShopException)
                    {
                        break;
                    }

                    if (info.Count < product.Value)
                    {
                        break;
                    }

                    totalSum += info.Price * product.Value;
                }

                if (totalSum != 0 && totalSum < minSum)
                {
                    minSum = totalSum;
                    cheapestShop = shop;
                }

                totalSum = 0;
            }

            return cheapestShop;
        }
    }
}