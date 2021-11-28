using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Builders.Clients;
using Banks.Enums;
using Banks.Models;
using Banks.Models.BankAccounts;

namespace Banks.ConsoleUI
{
    public class Application
    {
        private const string Prefix = "/";
        private readonly Dictionary<string, Action> _commands = new ();
        private readonly BaseBank _baseBank = BaseBank.GetInstance();

        public void Start()
        {
            Console.WriteLine(">>> Bank System <<<");

            InitCommands();

            Console.WriteLine("Available commands: ");
            _commands.Keys
                .ToList()
                .ForEach(Console.WriteLine);

            Console.WriteLine("\nType \"exit\" to exit\n");

            while (true)
            {
                string command = UserInput.GetLine()[0];

                if (_commands.ContainsKey(command))
                {
                    _commands[command].Invoke();
                }
                else
                {
                    Console.WriteLine("Command wasn't found");
                    command = UserInput.GetLine()[0];
                }

                if (command.ToLower() == "exit")
                    break;
            }
        }

        private void CreateBank()
        {
            Console.WriteLine("Write bank's name:");
            string name = UserInput.GetLine()[0];

            Console.WriteLine("Write interest rate, commission rate, operation limit and credit limit accordingly: ");

            List<decimal> bankData = UserInput.GetDecimalArgs();
            while (bankData.Count != 4)
            {
                Console.WriteLine("Invalid count of arguments. Try again: ");
                bankData = UserInput.GetDecimalArgs();
            }

            _baseBank.CreateBank(name, bankData[0], bankData[1], bankData[2], bankData[3]);
            Console.WriteLine("New bank was created");
        }

        private void CreateAccount()
        {
            Console.WriteLine("Enter client's name and lastname: ");

            List<string> input = UserInput.GetLine();
            while (input.Count != 2)
            {
                Console.WriteLine("Input must contain both name and lastname. Try again: ");
                input = UserInput.GetLine();
            }

            Client client = new ClientBuilder()
                .SetPersonalData(input[0], input[1])
                .Build();

            Console.WriteLine($"Client's id: {client.Id}");

            Console.WriteLine("Enter bank's name to add the client to: ");
            string name = UserInput.GetLine()[0];
            Bank bank = GetBank(name);

            Console.WriteLine("What kind of account to create?");
            bool flag = true;
            while (flag)
            {
                string type = UserInput.GetLine()[0].ToLower();
                switch (type)
                {
                    case "credit":
                        var credit = _baseBank.CreateAccount(bank, client, AccountType.Credit);
                        Console.WriteLine($"Account's id: {credit.Id}");
                        Console.WriteLine($"Credit account with data: {client.FirstName}, {client.LastName}" +
                                          "was created successfully");
                        flag = false;
                        break;
                    case "debit":
                        var debit = _baseBank.CreateAccount(bank, client, AccountType.Debit);
                        Console.WriteLine($"Account's id: {debit.Id}");
                        Console.WriteLine($"Debit account with data: {client.FirstName}, {client.LastName}" +
                                          "was created successfully");
                        flag = false;
                        break;
                    case "deposit":
                        var deposit = _baseBank.CreateAccount(bank, client, AccountType.Deposit);
                        Console.WriteLine($"Account's id: {deposit.Id}");
                        Console.WriteLine($"Deposit account with data: {client.FirstName}, {client.LastName}" +
                                          "was created successfully");
                        flag = false;
                        break;
                    default:
                        Console.WriteLine($"Account type {type} doesn't exist. Try again: ");
                        continue;
                }
            }
        }

        private void AccountOperation()
        {
            Console.WriteLine("Enter client's bank name: ");
            string name = UserInput.GetLine()[0];

            Bank bank = GetBank(name);

            Console.WriteLine("Type client's id: ");
            string guid = UserInput.GetLine()[0];

            Client client = GetClient(bank, guid);

            Console.WriteLine("Type account's id: ");
            string id = UserInput.GetLine()[0];

            IBankAccount account = GetBankAccount(bank, client, id);

            Console.WriteLine("Enter type of operation (withdraw, replenish, transfer): ");
            string type = UserInput.GetLine()[0].ToLower();

            Console.WriteLine("Enter sum: ");
            decimal sum = UserInput.GetDecimalArgs()[0];

            bool flag = true;
            while (flag)
            {
                switch (type)
                {
                    case "withdraw":
                        _baseBank.PerformTransaction(account, sum, TransactionType.Withdraw);
                        Console.WriteLine("Withdrawal has been completed");
                        flag = false;
                        break;
                    case "replenish":
                        _baseBank.PerformTransaction(account, sum, TransactionType.Replenish);
                        Console.WriteLine("Replenish has been completed");
                        flag = false;
                        break;
                    case "transfer":
                        Console.WriteLine("Enter second client's bank name: ");
                        string secondName = UserInput.GetLine()[0];

                        Bank secondBank = GetBank(secondName);

                        Console.WriteLine("Type second client's id: ");
                        string secondGuid = UserInput.GetLine()[0];

                        Client secondClient = GetClient(secondBank, secondGuid);

                        Console.WriteLine("Type second account's id: ");
                        string secondId = UserInput.GetLine()[0];

                        IBankAccount secondAccount = GetBankAccount(secondBank, secondClient, secondId);

                        _baseBank.Transfer(account, secondAccount, sum);
                        Console.WriteLine("Transfer has been completed");
                        flag = false;
                        break;
                    default:
                        Console.WriteLine($"Operation with type: {type} wasn't fount. Try again: ");
                        type = UserInput.GetLine()[0].ToLower();
                        break;
                }
            }
        }

        private void DeclineOperation()
        {
            Console.WriteLine("Enter client's bank name: ");
            string name = UserInput.GetLine()[0];

            Bank bank = GetBank(name);

            Console.WriteLine("Type client's id: ");
            string guid = UserInput.GetLine()[0];

            Client client = GetClient(bank, guid);

            Console.WriteLine("Type account's id: ");
            string id = UserInput.GetLine()[0];

            IBankAccount account = GetBankAccount(bank, client, id);

            Console.WriteLine("Type transaction's id: ");
            string transactionId = UserInput.GetLine()[0];

            account.History.GetTransaction(Guid.Parse(transactionId)).Decline();
            Console.WriteLine($"Transaction with id: {transactionId} was declined successfully");
        }

        private void GetAccountBalance()
        {
            Console.WriteLine("Enter client's bank name: ");
            string name = UserInput.GetLine()[0];

            Bank bank = GetBank(name);

            Console.WriteLine("Type client's id: ");
            string guid = UserInput.GetLine()[0];

            Client client = GetClient(bank, guid);

            Console.WriteLine("Type account's id: ");
            string id = UserInput.GetLine()[0];

            IBankAccount account = GetBankAccount(bank, client, id);
            Console.WriteLine($"Balance: {account.GetBalance()}");
        }

        private void ChangePercentageRate()
        {
            Console.WriteLine("Enter bank's name: ");
            string name = UserInput.GetLine()[0];

            Bank bank = GetBank(name);

            Console.WriteLine("Enter new percentage rate: ");
            decimal rate = UserInput.GetDecimalArgs()[0];

            bank.PercentageRate = rate;
            Console.WriteLine($"{bank.Name}'s bank percentage rate was changed to: {rate} successfully");
        }

        private void ChangeCommissionRate()
        {
            Console.WriteLine("Enter bank's name: ");
            string name = UserInput.GetLine()[0];

            Bank bank = GetBank(name);

            Console.WriteLine("Enter new commission rate: ");
            decimal rate = UserInput.GetDecimalArgs()[0];

            bank.CommissionRate = rate;
            Console.WriteLine($"{bank.Name}'s bank commission rate was changed to: {rate} successfully");
        }

        private void ChangeTransactionLimit()
        {
            Console.WriteLine("Enter bank's name: ");
            string name = UserInput.GetLine()[0];

            Bank bank = GetBank(name);

            Console.WriteLine("Enter new transaction limit: ");
            decimal limit = UserInput.GetDecimalArgs()[0];

            bank.TransactionLimit = limit;
            Console.WriteLine($"{bank.Name}'s bank transaction limit rate was changed to: {limit} successfully");
        }

        private void ChangeCreditLimit()
        {
            Console.WriteLine("Enter bank's name: ");
            string name = UserInput.GetLine()[0];

            Bank bank = GetBank(name);

            Console.WriteLine("Enter new credit limit: ");
            decimal limit = UserInput.GetDecimalArgs()[0];

            bank.CreditLimit = limit;
            Console.WriteLine($"{bank.Name}'s bank credit limit rate was changed to: {limit} successfully");
        }

        private Bank GetBank(string name)
        {
            Bank bank = _baseBank.Banks.FirstOrDefault(b => b.Name == name);
            while (bank is null)
            {
                Console.WriteLine($"The bank with name: {name} doesn't exist. Try again: ");
                name = UserInput.GetLine()[0];
                bank = _baseBank.Banks.FirstOrDefault(b => b.Name == name);
            }

            return bank;
        }

        private Client GetClient(Bank bank, string guid)
        {
            Client client = _baseBank.BankClients(bank).FirstOrDefault(c => c.Id.ToString() == guid);
            while (client is null)
            {
                Console.WriteLine($"Client with id: {guid} doesn't exist. Try again: ");
                guid = UserInput.GetLine()[0];
                client = _baseBank.BankClients(bank).FirstOrDefault(c => c.Id.ToString() == guid);
            }

            return client;
        }

        private IBankAccount GetBankAccount(Bank bank, Client client, string id)
        {
            IBankAccount account = bank.GetClients()[client].FirstOrDefault(a => a.Id.ToString() == id);
            while (account is null)
            {
                Console.WriteLine($"Account with id: {id} doesn't exist. Try again: ");
                id = UserInput.GetLine()[0];
                account = bank.GetClients()[client].FirstOrDefault(a => a.Id.ToString() == id);
            }

            return account;
        }

        private void InitCommands()
        {
            _commands.Add($"{Prefix}create_bank", CreateBank);
            _commands.Add($"{Prefix}create_account", CreateAccount);
            _commands.Add($"{Prefix}perform_account_operation", AccountOperation);
            _commands.Add($"{Prefix}decline_account_operation", DeclineOperation);
            _commands.Add($"{Prefix}get_account_balance", GetAccountBalance);
            _commands.Add($"{Prefix}change_banks_credit_limit", ChangeCreditLimit);
            _commands.Add($"{Prefix}change_banks_transaction_limit", ChangeTransactionLimit);
            _commands.Add($"{Prefix}change_banks_commission_rate", ChangeCommissionRate);
            _commands.Add($"{Prefix}change_banks_percentage_rate", ChangePercentageRate);
            _commands.Add($"{Prefix}inform_commission", () => _baseBank.InformBanks(BankEventType.Commission));
            _commands.Add($"{Prefix}inform_percentage", () => _baseBank.InformBanks(BankEventType.Percentage));
        }
    }
}