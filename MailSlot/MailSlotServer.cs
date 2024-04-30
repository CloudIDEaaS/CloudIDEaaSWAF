using System;
using System.Text;

namespace MailSlot
{
    public class MailslotServer : IDisposable
    {
        private MailSlot Slot;
        public const int MAILSLOT_WAIT_FOREVER = -1;


        public MailslotServer(string name, string domain = ".", int readTimeoutMilliseconds = MAILSLOT_WAIT_FOREVER)
        {
            Slot = MailSlot.ForServer(domain, name, readTimeoutMilliseconds);
        }

        public string GetNextMessage()
        {
            var bytes = Slot.GetNextMessage();

            if (bytes != null)
            {
                return Encoding.UTF8.GetString(bytes);
            }

            return null;
        }

        public void Dispose()
        {
            if (Slot != null)
            {
                ((IDisposable)Slot).Dispose();
            }
        }
    }
}
