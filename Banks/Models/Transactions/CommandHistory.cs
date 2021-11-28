using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Tools;

namespace Banks.Models.Transactions
{
    public class CommandHistory
    {
        private readonly List<ITransaction> _transactions;

        public CommandHistory()
        {
            _transactions = new List<ITransaction>();
        }

        public void AddTransaction(ITransaction transaction) => _transactions.Add(transaction);

        public ITransaction GetTransaction(Guid id)
               => _transactions.FirstOrDefault(t => t.Id == id)
               ?? throw new BanksException($"Transaction with id: {id} wasn't found");
    }
}