using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Enums;
using Banks.Models.BankAccounts;
using Banks.Models.Notifications;
using Banks.Tools;

namespace Banks.Models
{
    public class Bank
    {
        private readonly Dictionary<Client, List<IBankAccount>> _clients;

        private decimal _percentageRate;
        private decimal _commissionRate;
        private decimal _operationLimit;
        private decimal _creditLimit;
        private DateTime _accountTerm;

        public Bank(
            BaseBank baseBank,
            string name,
            decimal percentageRate,
            decimal commissionRate,
            decimal transactionLimit,
            decimal creditLimit)
        {
            Name = name;
            _clients = new Dictionary<Client, List<IBankAccount>>();
            PercentageRate = percentageRate;
            CommissionRate = commissionRate;
            TransactionLimit = transactionLimit;
            CreditLimit = creditLimit;

            baseBank.RaiseInformEvent += HandleInformEvent;
        }

        public event EventHandler<ClientNotification> RaiseAccountChangeEvent;

        public string Name { get; }

        public decimal PercentageRate
        {
            get => _percentageRate;

            internal set
            {
                if (value <= 0)
                    throw new BanksException("Percentage rate must be greater than zero");

                InformClients($"New percentage rate: {value}");
                _percentageRate = value;
            }
        }

        public decimal CommissionRate
        {
            get => _commissionRate;

            internal set
            {
                if (value <= 0)
                    throw new BanksException("Commission rate must be greater than zero");

                InformClients($"New commission rate: {value}");
                _commissionRate = value;
            }
        }

        public decimal TransactionLimit
        {
            get => _operationLimit;

            internal set
            {
                if (value <= 0)
                    throw new BanksException("Transaction limit must be greater than zero");

                InformClients($"New Transaction limit for suspicious accounts: {value}");
                _operationLimit = value;

                _clients.Values
                    .SelectMany(l => l)
                    .ToList()
                    .ForEach(a => a.ChangeTransactionLimit(_operationLimit));
            }
        }

        public decimal CreditLimit
        {
            get => _creditLimit;

            internal set
            {
                if (value <= 0)
                    throw new BanksException("Credit limit must be greater than zero");

                InformClients($"New credit limit: {value}");
                _creditLimit = value;
            }
        }

        public DateTime AccountTerm
        {
            get => _accountTerm;

            internal set
            {
                InformClients($"New account term: {value}");
                _accountTerm = value;
            }
        }

        public void CreateAccount(Client client, IBankAccount account)
        {
            _ = client ?? throw new BanksException("Invalid client reference");
            _ = account ?? throw new BanksException("Invalid account reference");

            if (_clients.ContainsKey(client))
            {
                _clients[client].Add(account);
            }
            else
            {
                client.AdditionalDataChanged += HandleClientDataChange;
                _clients.Add(client, new List<IBankAccount> { account });
            }
        }

        internal void InformClients(string message) => OnAccountChangeEvent(new ClientNotification(message));
        internal IReadOnlyDictionary<Client, List<IBankAccount>> GetClients() => _clients;

        private void HandleInformEvent(object sender, BankNotification info)
        {
            switch (info.Event)
            {
                case BankEventType.Commission:
                    _clients.Values
                        .SelectMany(l => l)
                        .ToList()
                        .ForEach(a => a.DecreaseBalance(a.GetBalance() * _commissionRate / 365));
                    break;
                case BankEventType.Percentage:
                    _clients.Values
                        .SelectMany(l => l)
                        .ToList()
                        .ForEach(a => a.IncreaseBalance(a.GetBalance() * _percentageRate / 365));
                    break;
                default:
                    throw new BanksException("Unknown bank event type");
            }
        }

        private void SkipTime(int dayCount) => _clients.Values
                .SelectMany(l => l)
                .ToList()
                .ForEach(a => a.IncreaseBalance(a.GetBalance() * _percentageRate / 365 * dayCount));

        private void HandleClientDataChange(object sender, EventArgs e)
        {
            var client = sender as Client;
            _ = client ?? throw new BanksException("Sender's type is not client");

            if (client.IsConfirmed())
            {
                _clients[client].ForEach(account => account.ChangeSuspicionState(false));
            }
            else
            {
                _clients[client].ForEach(account => account.ChangeSuspicionState(true));
            }
        }

        private void OnAccountChangeEvent(ClientNotification info) => RaiseAccountChangeEvent?.Invoke(this, info);
    }
}