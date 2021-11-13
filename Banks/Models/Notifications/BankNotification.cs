using Banks.Enums;

namespace Banks.Models.Notifications
{
    public class BankNotification
    {
        public BankNotification(BankEventType bankEvent)
        {
            Event = bankEvent;
        }

        internal BankEventType Event { get; }
    }
}