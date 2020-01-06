using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System;

public class Serializer
{
    public Order orderType;

    public Serializer()
    {
        this.orderType = new Order();
    }

    public byte[] Serialize_Obj(object toSerialize)
    {
        if (toSerialize == null)
        {
            return null;
        }
        XmlSerializer serializer = new XmlSerializer(toSerialize.GetType());

        using (var memoryStream = new MemoryStream())
        {
            serializer.Serialize(memoryStream, toSerialize);
            return memoryStream.ToArray();
        }
    }

    public object Deserialize_Obj(byte[] toDeserialize, object type)
    {
        MemoryStream ms = new MemoryStream(toDeserialize);
        XmlSerializer xmlSerializer = new XmlSerializer(type.GetType(), "");
        object testObj = xmlSerializer.Deserialize(ms);

        return testObj;

    }

}

