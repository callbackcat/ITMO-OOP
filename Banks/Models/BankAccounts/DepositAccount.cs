using System;
using Banks.Models.Transactions;
using Banks.Tools;

namespace Banks.Models.BankAccounts
{
    public class DepositAccount : IBankAccount
    {
        private readonly CommandHistory _history;
        private readonly DateTime _accountTerm;

        private decimal _balance;
        private decimal _transactionLimit;
        private bool _suspicious = true;

        public DepositAccount(Bank bank, decimal balance)
        {
            _balance = balance;
            Id = Guid.NewGuid();
            _accountTerm = bank.AccountTerm;
            _history = new CommandHistory();
            _transactionLimit = bank.TransactionLimit;
        }

        public Guid Id { get; }

        public void DecreaseBalance(decimal sum)
        {
            // Abs to handle potential miss input
            if (_balance < Math.Abs(sum))
                throw new BanksException("Insufficient funds on the account");

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

            if (!IsExpired())
                throw new BanksException("The deposit account has not expired");

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
        public decimal GetBalance() => _balance;
        private bool IsExpired() => _accountTerm < DateTime.Now;
    }
}