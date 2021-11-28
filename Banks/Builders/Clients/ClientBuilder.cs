using Banks.Models;
using Banks.Tools;

namespace Banks.Builders.Clients
{
    public class ClientBuilder : IClientBuilder
    {
        private Client _client;

        public ClientBuilder SetPersonalData(string firstName, string lastName)
        {
            _client = new Client(firstName, lastName);
            return this;
        }

        public ClientBuilder SetAddress(string address)
        {
            ClientHasPersonalData();
            _client.Address = address;
            return this;
        }

        public ClientBuilder SetPassport(string passport)
        {
            ClientHasPersonalData();
            _client.Passport = passport;
            return this;
        }

        public Client Build()
        {
            ClientHasPersonalData();
            Client resultClient = _client;
            Reset();
            return resultClient;
        }

        private void Reset() => _client = null;
        private void ClientHasPersonalData()
            => _ = _client ?? throw new BanksException("The client must have personal data");
    }
}