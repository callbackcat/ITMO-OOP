using System;
using Banks.Models.Transactions;

namespace Banks.Models.BankAccounts
{
    public interface IBankAccount
    {
        Guid Id { get; }
        CommandHistory History { get; }
        void IncreaseBalance(decimal sum);
        void DecreaseBalance(decimal sum);

        void Withdraw(decimal sum);
        void Replenish(decimal sum);
        void ChangeSuspicionState(bool state);
        void ChangeTransactionLimit(decimal limit);
        decimal GetBalance();
    }
}