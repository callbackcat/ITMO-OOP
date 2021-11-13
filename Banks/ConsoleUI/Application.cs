using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Builders.Clients;
using Banks.Enums;
using Banks.Models;
using Banks.Models.BankAccounts;
using Banks.Tools;

namespace Banks.ConsoleUI
{
    public class Application
    {
        private const string Prefix = "/";

        private readonly Dictionary<string, Action> _commands = new ();
        private readonly BaseBank _baseBank = BaseBank.GetInstance();

        public void Start()
        {
            Console.WriteLine(">>> Cringe UI | Bonk System v0.1 <<<");
            Console.WriteLine("Available commands: ");

            _commands.Add("/create_bank", CreateBank);
            _commands.Add("/create_client", CreateClient);
            _commands.Add("/inform_commission", () => _baseBank.InformBanks(BankEventType.Commission));
            _commands.Add("/inform_percentage", () => _baseBank.InformBanks(BankEventType.Percentage));

            _commands.Keys
                .ToList()
                .ForEach(Console.WriteLine);

            Console.WriteLine("\nType \"exit\" to exit\n");

            while (true)
            {
                string command = UserInput.Get()[0];

                if (_commands.ContainsKey(command))
                {
                    _commands[command].Invoke();
                }
                else
                {
                    Console.WriteLine("Command wasn't found");
                }

                if (command.ToLower() == "exit")
                    break;
            }
        }

        public void AddCommand(string name, Action action) => _commands.Add($"{Prefix}{name}", action);

        private void CreateBank()
        {
            Console.WriteLine("Write bank's name:");
            string name = UserInput.Get()[0];

            Console.WriteLine("Write interest rate, commission rate, operation limit and credit limit accordingly");
            string[] input = UserInput.Get();
            decimal[] bankData = Array.ConvertAll(input, decimal.Parse);

            _baseBank.CreateBank(name, bankData[0], bankData[1], bankData[2], bankData[3]);
            Console.WriteLine("New bank was created");
        }

        private void CreateClient()
        {
            Console.WriteLine("Enter client's name and lastname: ");
            string[] input = UserInput.Get();
            string clientName = input[0];
            string clientLastname = input[1];

            Client testClient = new ClientBuilder()
                .SetPersonalData($"{clientName}", $"{clientLastname}")
                .Build();

            Console.WriteLine("Client was created");
        }
    }
}