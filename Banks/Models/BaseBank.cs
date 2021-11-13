using System;
using System.Collections.Generic;
using Banks.Enums;
using Banks.Factories;
using Banks.Models.BankAccounts;
using Banks.Models.Notifications;
using Banks.Models.Transactions;
using Banks.Tools;

namespace Banks.Models
{
    public sealed class BaseBank
    {
        private static BaseBank _instance;
        private readonly List<Bank> _banks;

        private BaseBank()
        {
            _banks = new List<Bank>();
        }

        public event EventHandler<BankNotification> RaiseInformEvent;

        public static BaseBank GetInstance() => _instance ??= new BaseBank();

        public Bank CreateBank(
            string name,
            decimal interestRate,
            decimal commissionRate,
            decimal operationLimit,
            decimal creditLimit)
        {
            var bank = new Bank(this, name, interestRate, commissionRate, operationLimit, creditLimit);
            _banks.Add(bank);
            return bank;
        }

        public void InformBanks(BankEventType bankEvent) => OnBankEvent(new BankNotification(bankEvent));

        public IBankAccount CreateAccount(Bank bank, Client client, AccountType type)
        {
            _ = bank ?? throw new BanksException("Invalid bank reference");
            _ = client ?? throw new BanksException("Invalid client reference");

            if (!IsRegisteredBank(bank))
                throw new BanksException("Unregistered bank");

            IBankAccount account = new AccountFactory().CreateAccount(type, bank);
            bank.CreateAccount(client, account);
            return account;
        }

        public void Transfer(IBankAccount accountFrom, IBankAccount accountTo, decimal sum)
        {
            _ = accountFrom ?? throw new BanksException("Invalid account reference");
            _ = accountTo ?? throw new BanksException("Invalid account reference");

            var transaction = new Transfer(accountFrom, accountTo, sum);
            transaction.Execute();
        }

        public void PerformTransaction(IBankAccount account, decimal sum, TransactionType type)
        {
            switch (type)
            {
                case TransactionType.Withdraw:
                    account.Withdraw(sum);
                    break;
                case TransactionType.Replenish:
                    account.Replenish(sum);
                    break;
                default:
                    throw new BanksException("Invalid transaction type");
            }
        }

        public IReadOnlyCollection<Client> BankClients(Bank bank) => bank.GetClients();
        private void OnBankEvent(BankNotification info) => RaiseInformEvent?.Invoke(this, info);
        private bool IsRegisteredBank(Bank bank) => _banks.Contains(bank);
    }
}