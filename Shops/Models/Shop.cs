using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Models.Products;
using Shops.Tools;

namespace Shops.Models
{
    public class Shop
    {
        private readonly Dictionary<Product, ProductInfo> _products;
        private string _name;
        private string _address;
        private Guid _id;

        public Shop(string name, string address)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ShopException("Invalid name");

            if (string.IsNullOrWhiteSpace(address))
                throw new ShopException("Invalid address");

            _name = name;
            _address = address;
            _id = Guid.NewGuid();
            _products = new Dictionary<Product, ProductInfo>();
        }

        public List<Product> AddProducts(Product product, ProductInfo info)
        {
            if (_products.ContainsKey(product))
            {
                _products[product].Count += info.Count;
            }
            else
            {
                if (info.Price <= 0)
                {
                    throw new ShopException($"The {product.Name} price is not valid");
                }

                if (info.Count == 0)
                {
                    throw new ShopException("The product count must not be zero");
                }

                _products.Add(product, info);
            }

            return new List<Product>(new Product[info.Count]);
        }

        public List<Product> Buy(Person client, params KeyValuePair<Product, int>[] products)
        {
            if (products.Any(p => p.Value <= 0))
            {
                throw new ShopException("The product's count must be greater than zero");
            }

            if (!products.All(p => _products.ContainsKey(p.Key))
                || products.Any(p => _products[p.Key].Count == 0))
            {
                throw new ShopException("The shop doesn't contain all the products");
            }

            double totalSum = products
                .Select(p => _products[p.Key].Price * p.Value)
                .Sum();

            if (client.Balance - totalSum < 0)
            {
                throw new ShopException("The client doesn't have enough money");
            }

            foreach (var product in products)
            {
                _products[product.Key].Count -= (uint)products
                    .Select(p => p.Value)
                    .Sum();
            }

            client.Balance -= totalSum;

            return products.Select(p => p.Key).ToList();
        }

        public KeyValuePair<Product, ProductInfo> GetProductInfo(Guid id)
        {
            if (_products.All(p => p.Key.Id != id))
            {
                throw new ShopException("The shop doesn't contain the product");
            }

            return _products.Single(p => p.Key.Id == id);
        }

        public Product ChangePrice(Product product, double newPrice)
        {
            if (product is null || !_products.ContainsKey(product))
            {
                throw new ShopException("The shop doesn't contain the product");
            }

            _products
                .Single(p => p.Key.Id == product.Id)
                .Value
                .Price = newPrice;

            return product;
        }
    }
}