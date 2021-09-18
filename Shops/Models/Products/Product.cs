using System;
using Shops.Tools;

namespace Shops.Models.Products
{
    public class Product
    {
        public Product(string name, Guid id)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ShopException("The product name is not valid");

            Name = name;
            Id = id;
        }

        public string Name { get; }
        public Guid Id { get; }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}