using System;
using Banks.Models.Transactions;
using Banks.Tools;

namespace Banks.Models.BankAccounts
{
    public class CreditAccount : IBankAccount
    {
        private readonly CommandHistory _history;

        private decimal _balance;
        private decimal _transactionLimit;
        private decimal _creditLimit;
        private bool _suspicious = true;

        public CreditAccount(Bank bank, decimal balance)
        {
            _balance = balance;
            Id = Guid.NewGuid();
            _history = new CommandHistory();
            _creditLimit = bank.CreditLimit;
            _transactionLimit = bank.TransactionLimit;
        }

        public Guid Id { get; }
        public CommandHistory History => _history;

        public void DecreaseBalance(decimal sum)
        {
            if (Math.Abs(_balance) < _creditLimit)
                throw new BanksException("Account's credit limit was reached");

            _balance -= Math.Abs(sum);
        }

        public void IncreaseBalance(decimal sum)
        {
            if (sum <= 0)
                throw new BanksException("Sum must be greater than zero");

            _balance += sum;
        }

        public void Withdraw(decimal sum)
        {
            if (_suspicious && sum > _transactionLimit)
            {
                throw new BanksException("Error. Suspicious account's" +
                                         $"withdrawal sum is limited by {_transactionLimit}");
            }

            var transaction = new Withdraw(this, sum);
            _history.AddTransaction(transaction);
            transaction.Execute();
        }

        public void Replenish(decimal sum)
        {
            var transaction = new Replenish(this, sum);
            _history.AddTransaction(transaction);
            transaction.Execute();
        }

        public void DeclineTransaction(Guid id) => _history
            .GetTransaction(id)
            .Decline();

        public void ChangeSuspicionState(bool state) => _suspicious = state;
        public void ChangeTransactionLimit(decimal limit) => _transactionLimit = limit;
        public void ChangeCreditLimit(decimal limit) => _creditLimit = limit;
        public decimal GetBalance() => _balance;
    }
}