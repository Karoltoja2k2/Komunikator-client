using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
[System.Xml.Serialization.XmlInclude(typeof(Order))]
public class Conversation
{
    public int you;
    public int receiver;
    public List<Order> messages;

    public Conversation()
    {

    }

    public Conversation(int yoursNr, int rcvNr)
    {
        you = yoursNr;
        receiver = rcvNr;
        messages = new List<Order>();
    }
}

