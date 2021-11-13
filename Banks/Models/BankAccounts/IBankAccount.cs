using System;

namespace Banks.Models.BankAccounts
{
    public interface IBankAccount
    {
        Guid Id { get; }
        void IncreaseBalance(decimal sum);
        void DecreaseBalance(decimal sum);

        void Withdraw(decimal sum);
        void Replenish(decimal sum);
        void ChangeSuspicionState(bool state);
        void ChangeTransactionLimit(decimal limit);
        decimal GetBalance();
    }
}