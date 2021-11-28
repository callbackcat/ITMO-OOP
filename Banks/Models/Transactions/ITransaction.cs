using System;
using Banks.Enums;

namespace Banks.Models.Transactions
{
    public interface ITransaction
    {
        Guid Id { get; }
        void Execute();
        void Decline();
        TransactionStatus Status();
    }
}