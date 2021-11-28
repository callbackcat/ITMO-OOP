using Banks.Models;
using Banks.Models.BankAccounts;

namespace Banks.Creators
{
    public class DepositAccountCreator : IAccountCreator
    {
        public IBankAccount CreateAccount(Bank bank, decimal balance = 0) => new DepositAccount(bank, balance);
    }
}