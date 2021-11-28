using Banks.Builders.Clients;
using Banks.Enums;
using Banks.Models;
using Banks.Models.BankAccounts;
using NUnit.Framework;

namespace Banks.Tests
{
    public class Tests
    {
        private BaseBank _baseBank;
        private Bank _bank;
        private Client _testClient;

        [SetUp]
        public void Setup()
        {
            const decimal interestRate = 10;
            const decimal commissionRate = 5;
            const decimal operationLimit = 1000;
            const decimal creditLimit = 1000;

            const string bankName = "Bonk";

            _baseBank = BaseBank.GetInstance();
            var builder = new ClientBuilder();

            _bank = _baseBank.CreateBank(bankName, interestRate, commissionRate, operationLimit, creditLimit);

            _testClient = builder
                .SetPersonalData("Makarevich", "Approve")
                .SetAddress("xD")
                .Build();
        }

        [Test]
        public void CreateBankAddUser_BankContainsUser()
        {
            _baseBank.CreateAccount(_bank, _testClient, AccountType.Debit);
            CollectionAssert.Contains(_baseBank.BankClients(_bank), _testClient);
        }

        [Test]
        public void ClientReplenishAccount_BalanceIncreased()
        {
            const decimal sum = 100;

            IBankAccount account = _baseBank.CreateAccount(_bank, _testClient, AccountType.Debit);
            _baseBank.PerformTransaction(account, sum, TransactionType.Replenish);

            Assert.AreEqual(sum, account.GetBalance());
        }

        [Test]
        public void ClientWithdrawAccount_BalanceDecreased()
        {
            const decimal sumToReplenish = 100;
            const decimal sumToWithdraw = 50;

            IBankAccount account = _baseBank.CreateAccount(_bank, _testClient, AccountType.Debit);
            _baseBank.PerformTransaction(account, sumToReplenish, TransactionType.Replenish);
            _baseBank.PerformTransaction(account, sumToWithdraw, TransactionType.Withdraw);

            Assert.AreEqual(sumToReplenish-sumToWithdraw, account.GetBalance());
        }

        [Test]
        public void TransferMoney_AccountFromBalanceDecreased_AccountToBalanceIncreased()
        {
            const decimal sumToReplenish = 100;

            IBankAccount accountFrom = _baseBank.CreateAccount(_bank, _testClient, AccountType.Debit);
            IBankAccount accountTo = _baseBank.CreateAccount(_bank, _testClient, AccountType.Debit);

            _baseBank.PerformTransaction(accountFrom, sumToReplenish, TransactionType.Replenish);
            _baseBank.Transfer(accountFrom, accountTo, sumToReplenish);

            Assert.AreEqual(sumToReplenish, accountTo.GetBalance());
        }
    }
}