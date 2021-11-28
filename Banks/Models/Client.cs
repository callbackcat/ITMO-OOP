using System;
using System.Runtime.CompilerServices;
using Banks.Models.Notifications;
using Banks.Tools;

namespace Banks.Models
{
    public class Client
    {
        private string _address;
        private string _passport;

        public Client(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new BanksException("Invalid client's first name");

            if (string.IsNullOrWhiteSpace(lastName))
                throw new BanksException("Invalid client's last name");

            FirstName = firstName;
            LastName = lastName;
            Id = Guid.NewGuid();
        }

        public event EventHandler AdditionalDataChanged;

        public Guid Id { get; }

        public string FirstName { get; }
        public string LastName { get; }

        public string Address
        {
            get => _address;

            internal set
            {
                if (value == _address) return;
                _address = value;
                OnAdditionalDataChanged();
            }
        }

        public string Passport
        {
            get => _passport;

            internal set
            {
                if (value == _passport) return;
                _passport = value;
                OnAdditionalDataChanged();
            }
        }

        public bool IsConfirmed() => Address != null && Passport != null;

        public void SubscribeToBank(Bank bank) => bank.RaiseAccountChangeEvent += HandleBankAccountEvent;
        private void OnAdditionalDataChanged() => AdditionalDataChanged?.Invoke(this, EventArgs.Empty);

        private void HandleBankAccountEvent(object sender, ClientNotification e)
            => Console.WriteLine($"Got new message from bank: {e.Message}");
    }
}