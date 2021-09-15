using System.Collections.Generic;
using Shops.Services;
using Shops.Models;
using NUnit.Framework;
using Shops.Models.Products;
using Shops.Tools;

namespace Shops.Tests
{
    public class Tests
    {
        private IShopManager _shopManager;

        [SetUp]
        public void Setup()
        {
            _shopManager = new ShopManager();
        }

        [Test]
        public void CreateNewShop_AddProducts_PersonCanBuyProducts()
        {
            const int moneyBefore = 100;
            const int productPrice = 10;
            const int productCount = 5;
            const int productToBuyCount = 1;

            Shop shop = _shopManager.Create("Test shop", "Test Address");
            Product product = _shopManager.RegisterProduct("Test product");
            var person = new Person("Test customer", moneyBefore);
            var productInfo = new ProductInfo(productPrice, productCount);
            
            shop.AddProducts(product, productInfo);
            shop.Buy(person, new KeyValuePair<Product, int>(product, productToBuyCount));

            Assert.AreEqual(moneyBefore - productPrice * productToBuyCount, person.Balance);
            Assert.AreEqual(productCount - productToBuyCount, 
                shop.GetProductInfo(product.Id).Value.Count);
        }
        
        [Test]
        public void ShopSetProductsPrice_ProductsPricesCanBeChanged()
        {
            const int productPrice = 100;
            const int newProductPrice = 1000;
            const int productCount = 10;
            
            Shop shop = _shopManager.Create("Test shop", "Test Address");
            Product product = _shopManager.RegisterProduct("Test product");
            var productInfo = new ProductInfo(productPrice, productCount);
            
            shop.AddProducts(product, new ProductInfo(productPrice, productCount));
            Assert.AreEqual(productInfo.Price, 
                shop.GetProductInfo(product.Id).Value.Price);

            shop.ChangePrice(product, newProductPrice);
            Assert.AreEqual(newProductPrice, 
                shop.GetProductInfo(product.Id).Value.Price);
        }

        [Test]
        public void FindShopWithTheBestPrice()
        {
            const int productPrice = 100;
            const int biggerProductPrice = productPrice * 2;
            const int productCount = 10;
            
            Shop cheapShop = _shopManager.Create("Cheap shop", "Test Address");
            Shop expensiveShop = _shopManager.Create("Expensive shop", "Test Address 2");
            Product product = _shopManager.RegisterProduct("Test product");
            
            cheapShop.AddProducts(product, new ProductInfo(productPrice, productCount));
            expensiveShop.AddProducts(product, new ProductInfo(productPrice, productCount));
            expensiveShop.ChangePrice(product, biggerProductPrice);
            
            Assert.AreEqual(cheapShop, _shopManager
                .FindShopWithLowestPrice(product.Id, productCount));
        }

        [Test]
        public void PersonBuyProducts_PersonHaveEnoughMoney_MoneyCountDecreased()
        {
            const int personBalance = 1000;
            const int productPrice = 100;
            const int productCount = 10;
            const int productToBuyCount = 5;
            
            Shop shop = _shopManager.Create("Test shop", "Test Address");
            Product product = _shopManager.RegisterProduct("Test product");
            var person = new Person("Test customer", personBalance);
                
            shop.AddProducts(product, new ProductInfo(productPrice, productCount));
            shop.Buy(person, new KeyValuePair<Product, int>(product, productToBuyCount));
            
            Assert.AreEqual(productCount - productToBuyCount, 
                shop.GetProductInfo(product.Id).Value.Count);
            
            Assert.AreEqual(personBalance - productPrice * productToBuyCount, 
                person.Balance);
            
            // Trying to buy products from empty shop
            shop.Buy(person, new KeyValuePair<Product, int>(product, productToBuyCount));
            Assert.Catch<ShopException>(() =>
            {
                shop.Buy(person, new KeyValuePair<Product, int>(product, productToBuyCount));
            });
        }
    }
}