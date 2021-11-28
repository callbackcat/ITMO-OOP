using Banks.Models;
using Banks.Models.BankAccounts;

namespace Banks.Creators
{
    public interface IAccountCreator
    {
        public IBankAccount CreateAccount(Bank bank, decimal balance);
    }
}