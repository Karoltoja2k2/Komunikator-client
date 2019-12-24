using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class User
    {
        public int accNumber;
        public string token;

        public User(int accNumber, string token)
        {
            this.accNumber = accNumber;
            this.token = token;
        }

        // public User(byte[] data)
        // {
        // }

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
}
