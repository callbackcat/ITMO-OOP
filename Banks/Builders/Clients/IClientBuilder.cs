namespace Banks.Builders.Clients
{
    public interface IClientBuilder
    {
        ClientBuilder SetPersonalData(string firstName, string lastName);
        ClientBuilder SetAddress(string address);
        ClientBuilder SetPassport(string passport);
    }
}