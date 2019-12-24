using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[Serializable]
public class Order
{
    public string token;
    public int sender;
    public int receiver;
    public string message;
    public DateTime timeSend;

    public Order()
    {

    }
    /// <summary>
    /// Send message to another client(receiver)
    /// </summary>
    public Order(string token, int sender, int receiver, string message, DateTime timeSend)
    {
        this.token = token;
        this.sender = sender;
        this.receiver = receiver;
        this.message = message;
        this.timeSend = timeSend;
    }
    /// <summary>
    /// Send friend request to another user(receiver)
    /// </summary>
    public Order(string token, int sender, int receiver, DateTime timeSend)
    {
        this.token = token;
        this.sender = sender;
        this.receiver = receiver;
        this.timeSend = timeSend;
    }

}


