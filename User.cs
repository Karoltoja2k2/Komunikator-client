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
    public string password;
    public string token;

    public List<Conversation> conversations;


    public User()
    {
    }


    public User(int accNumber, string email, string password, string token)
    {
        conversations = new List<Conversation>();
        this.accNumber = accNumber;
        this.email = email;
        this.password = password;
        this.token = token;
    }


    public byte[] ToByteArray()
    {
        string stringAccNumber = accNumber.ToString();

        byte[] byteAccNumber = Encoding.ASCII.GetBytes(stringAccNumber);

        List<byte> byteList = new List<byte>();
        byteList.AddRange(byteAccNumber);
        byteList.AddRange(Encoding.ASCII.GetBytes(";"));
        byteList.AddRange(Encoding.ASCII.GetBytes(token));
        return byteList.ToArray();
    }
}

