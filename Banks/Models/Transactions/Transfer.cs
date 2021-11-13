using System;
using Banks.Enums;
using Banks.Models.BankAccounts;
using Banks.Tools;

namespace Banks.Models.Transactions
{
    public class Transfer : ITransaction
    {
        private readonly IBankAccount _accountFrom;
        private readonly IBankAccount _accountTo;

        private readonly decimal _sum;
        private TransactionStatus _status = TransactionStatus.Declined;

        public Transfer(IBankAccount accountFrom, IBankAccount accountTo, decimal sum)
        {
            if (sum <= 0)
                throw new BanksException("Withdrawal sum must be greater than zero");

            _sum = sum;
            Id = Guid.NewGuid();
            _accountFrom = accountFrom ?? throw new BanksException("Invalid bank account reference");
            _accountTo = accountTo ?? throw new BanksException("Invalid bank account reference");
        }

        public Guid Id { get; }

        public void Execute()
        {
            _status = TransactionStatus.Accepted;
            _accountFrom.DecreaseBalance(_sum);
            _accountTo.IncreaseBalance(_sum);
        }

        public void Decline()
        {
            if (_status is TransactionStatus.Declined)
                throw new BanksException("Transaction can't be declined");

            _status = TransactionStatus.Declined;
            _accountFrom.IncreaseBalance(_sum);
            _accountTo.DecreaseBalance(_sum);
        }

        public TransactionStatus Status() => _status;
    }
}