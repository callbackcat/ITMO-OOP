using Shops.Tools;

namespace Shops.Models
{
    public class Person
    {
        private string _name;

        public Person(string name, double balance)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ShopException("The person's name is not valid");

            if (balance < 0)
                throw new ShopException("The balance is not valid");

            _name = name;
            Balance = balance;
        }

        public double Balance { get; set; }
    }
}