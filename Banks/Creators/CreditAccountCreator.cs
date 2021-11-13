using Banks.Models;
using Banks.Models.BankAccounts;

namespace Banks.Creators
{
    public class CreditAccountCreator : IAccountCreator
    {
        public IBankAccount CreateAccount(Bank bank, decimal balance = 0) => new CreditAccount(bank, balance);
    }
}