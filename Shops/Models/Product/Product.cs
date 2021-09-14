using System;
using Shops.Tools;

namespace Shops.Models.Product
{
    public class Product
    {
        public Product(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ShopException("The product name is not valid");

            Name = name;
            Id = Guid.NewGuid();
        }

        public string Name { get; }
        public Guid Id { get; }
    }
}