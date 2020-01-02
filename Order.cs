using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[Serializable]
[System.Xml.Serialization.XmlInclude(typeof(User))]
public class Order
{
    public enum OrderType
    {
        Message, FriendRequest
    }

    public int orderType;
    public string token;
    public int sender;
    public int receiver;
    public string message;
    public DateTime timeSend;
    public string password;
    public string email;
    public bool succes;
    public byte[] acc;

    public Order()
    {

    }

    /// <summary>
    /// Send message to another client(receiver) ORDERTYPE = 0
    /// </summary>
    public Order(int orderType, string token, int sender, int receiver, string message, DateTime timeSend)
    {
        this.orderType = orderType;
        this.token = token;
        this.sender = sender;
        this.receiver = receiver;
        this.message = message;
        this.timeSend = timeSend;
    }

    /// <summary>
    /// Send friend request to another user(receiver) if orderType == 1, or accept friend request if ordertype == 2
    /// </summary>
    public Order(int orderType, string token, int sender, int receiver, DateTime timeSend)
    {
        this.orderType = orderType;
        this.token = token;
        this.sender = sender;
        this.receiver = receiver;
        this.timeSend = timeSend;
    }

    /// <summary>
    /// login to server ORDERYPE = 3
    /// </summary>
    public Order(int orderType, int sender, string password)
    {
        this.orderType = orderType;
        this.sender = sender;
        this.password = password;
    }

    /// <summary>
    /// register ORDERTYPE = 4
    /// </summary>
    public Order(int orderType, int sender, string email, string password)
    {
        this.orderType = orderType;
        this.sender = sender;
        this.email = email;
        this.password = password;
    }

    /// <summary>
    /// response to client whether register was succesful ORDERTYPE = 5
    /// </summary>
    public Order(int orderType, int receiver, bool succes)
    {
        this.orderType = orderType;
        this.receiver = receiver;
        this.succes = succes;
    }

    /// <summary>
    /// ordertype == 6, after successful login send to client his profile, if not validated send succes as false
    /// </summary>
    public Order(int orderType, int receiver, bool succes, byte[] acc = null)
    {
        this.orderType = orderType;
        this.receiver = receiver;
        this.succes = succes;
        this.acc = acc;
    }

}


