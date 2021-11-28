using System;
using Banks.Enums;
using Banks.Models.BankAccounts;
using Banks.Tools;

namespace Banks.Models.Transactions
{
    public class Replenish : ITransaction
    {
        private readonly IBankAccount _receiver;
        private readonly decimal _sum;
        private TransactionStatus _status = TransactionStatus.Declined;

        public Replenish(IBankAccount receiver, decimal sum)
        {
            if (sum <= 0)
                throw new BanksException("Withdrawal sum must be greater than zero");

            _sum = sum;
            Id = Guid.NewGuid();
            _receiver = receiver ?? throw new BanksException("Invalid bank account reference");
        }

        public Guid Id { get; }

        public void Execute()
        {
            _status = TransactionStatus.Accepted;
            _receiver.IncreaseBalance(_sum);
        }

        public void Decline()
        {
            if (_status is TransactionStatus.Declined)
                throw new BanksException("Transaction can't be declined");

            _status = TransactionStatus.Declined;
            _receiver.DecreaseBalance(_sum);
        }

        public TransactionStatus Status() => _status;
    }
}