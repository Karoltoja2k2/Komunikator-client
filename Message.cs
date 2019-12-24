using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    [Serializable]
    public class Message
    {
        public int sender;
        public int receiver;
        public string message;
        public DateTime timeSend;

        public Message(int sender, int receiver, string message, DateTime timeSend)
        {
            this.sender = sender;
            this.receiver = receiver;
            this.message = message;
            this.timeSend = timeSend;
        }
    }
}
