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
            const int productToBuyCount = 2;

            Shop shop = _shopManager.Create("Test shop", "Test Address");
            Product product = _shopManager.RegisterProduct("Test product");
            var person = new Person("Test customer", moneyBefore);
            var productInfo = new ProductInfo(productPrice, productCount);
            
            var shoppingList = new ShoppingList();
            shoppingList.AddToList(product, productToBuyCount);
            
            shop.AddProducts(product, productInfo);
            shop.Buy(person, shoppingList);

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
            const int cakePrice = 1000;
            const int muffinPrice = 100;
            const int biggerCakePrice = cakePrice * 2;
            const int biggerMuffinPrice = muffinPrice * 2;
            const int cakeCount = 2;
            const int muffinCount = 5;
            
            Shop cheapShop = _shopManager.Create("Cheap shop", "Test Address");
            Shop expensiveShop = _shopManager.Create("Expensive shop", "Test Address 2");
            Product cake = _shopManager.RegisterProduct("Cake");
            Product muffin = _shopManager.RegisterProduct("Muffin");
            
            var shoppingList = new ShoppingList();
            shoppingList.AddToList(cake, cakeCount);
            shoppingList.AddToList(muffin, muffinCount);
            
            cheapShop.AddProducts(cake, new ProductInfo(cakePrice, cakeCount));
            cheapShop.AddProducts(muffin, new ProductInfo(muffinPrice, muffinCount));
            
            expensiveShop.AddProducts(cake, new ProductInfo(biggerCakePrice, cakeCount));
            expensiveShop.AddProducts(muffin, new ProductInfo(biggerMuffinPrice, muffinCount));
            
            Assert.AreEqual(cheapShop, _shopManager
                .FindShopWithLowestPrice(shoppingList));
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
            
            var shoppingList = new ShoppingList();
            shoppingList.AddToList(product, productToBuyCount);
                
            shop.AddProducts(product, new ProductInfo(productPrice, productCount));
            shop.Buy(person, shoppingList);
            
            Assert.AreEqual(productCount - productToBuyCount, 
                shop.GetProductInfo(product.Id).Value.Count);
            
            Assert.AreEqual(personBalance - productPrice * productToBuyCount, 
                person.Balance);
            
            // Trying to buy products from empty shop
            shop.Buy(person, shoppingList);
            Assert.Catch<ShopException>(() =>
            {
                shop.Buy(person, shoppingList);
            });
        }
    }
}