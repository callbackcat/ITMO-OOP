using Banks.Tools;

namespace Banks.Models.Notifications
{
    public class ClientNotification
    {
        public ClientNotification(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new BanksException("Invalid message");

            Message = message;
        }

        internal string Message { get; }
    }
}