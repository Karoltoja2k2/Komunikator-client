﻿using System;
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
    public User acc;
    public List<Order> messages;
    public List<Order> pendingOrders;
    public List<int> friendList;
    public string helpMsg;
    public int accNum;
    public int phoneNum;
    public string nickName;
    public List<User> foundProfiles;


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
    public Order(int orderType, int receiver, bool succes, List<Order> messages = null, User acc = null, List<Order> pOrders = null, List<int> fList = null, string msg = null)
    {
        this.orderType = orderType;
        this.receiver = receiver;
        this.succes = succes;
        this.messages = messages;
        this.acc = acc;
        this.pendingOrders = pOrders;
        this.friendList = fList;
        this.helpMsg = msg;
    }

    /// <summary>
    /// ordertype == 7, search user
    /// </summary>
    public Order(int orderType, int sender, int accNum = 0, int phoneNum = 0, string nickName = null, string email = null)
    {
        this.orderType = orderType;
        this.sender = sender;
        this.accNum = accNum;
        this.phoneNum = phoneNum;
        this.nickName = nickName;
        this.email = email;
    }

    /// <summary>
    /// ordertype == 8, send back search matched accounts
    /// </summary>
    public Order(int orderType, int receiver, List<User> foundProfiles)
    {
        this.orderType = orderType;
        this.receiver = receiver;
        this.foundProfiles = foundProfiles;
    }

    /// <summary>
    /// ordertype == 9, update profile info in DB
    /// </summary>
    public Order(int orderType, int sender, string email, string nickName, int phoneNum)
    {
        this.orderType = orderType;
        this.sender = sender;
        this.email = email;
        this.nickName = nickName;
        this.phoneNum = phoneNum;
    }
}


