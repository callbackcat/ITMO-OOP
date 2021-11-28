using Banks.Models;
using Banks.Models.BankAccounts;

namespace Banks.Creators
{
    public class DebitAccountCreator : IAccountCreator
    {
        public IBankAccount CreateAccount(Bank bank, decimal balance = 0) => new DebitAccount(bank, balance);
    }
}