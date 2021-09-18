using System;
using Shops.Models;
using Shops.Models.Products;

namespace Shops.Services
{
    public interface IShopManager
    {
        Shop Create(string name, string address);
        Product RegisterProduct(string name);
        Shop FindShopWithLowestPrice(ShoppingList list);
        Shop FindShop(string name, string address);
    }
}