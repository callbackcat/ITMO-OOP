using Banks.Creators;
using Banks.Enums;
using Banks.Models;
using Banks.Models.BankAccounts;
using Banks.Tools;

namespace Banks.Factories
{
    public class AccountFactory
    {
        public IBankAccount CreateAccount(AccountType type, Bank bank)
        {
            return type switch
            {
                AccountType.Debit => new DebitAccountCreator().CreateAccount(bank),
                AccountType.Deposit => new DepositAccountCreator().CreateAccount(bank),
                AccountType.Credit => new CreditAccountCreator().CreateAccount(bank),
                _ => throw new BanksException($"The creator's type {nameof(type)} wasn't found")
            };
        }
    }
}