using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
[System.Xml.Serialization.XmlInclude(typeof(Conversation))]
[System.Xml.Serialization.XmlInclude(typeof(Order))]
public class User
{
    public int accNumber;
    public string email;
    public string token;

    public string nickName;
    public int phoneNum;
    public string city;
    public string country;

    public List<Conversation> conversations;
    public List<int> friendList;
    public List<Order> pendingOrders;


    public User()
    {
    }


    public User(int accNumber, string email, string token)
    {
        conversations = new List<Conversation>();
        this.accNumber = accNumber;
        this.email = email;
        this.token = token;
    }

    public void updateProfile(string nick, int pNum, string City, string Country)
    {
        this.nickName = nick;
        this.phoneNum = pNum;
        this.city = City;
        this.country = Country;
    }
}

