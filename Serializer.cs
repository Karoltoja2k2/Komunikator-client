using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Serializer
    {
        public byte[] Serialize_Obj(object toSerialize)
        {
            if (toSerialize == null)
            {
                return null;
            }
            var bF = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                bF.Serialize(memoryStream, toSerialize);
                return memoryStream.ToArray();
            }
        }

        public T Deserialize_Obj<T>(byte[] toDeserialize) where T : class
        {
            using (var memoryStream = new MemoryStream())
            {
                IFormatter bF = new BinaryFormatter();
                memoryStream.Write(toDeserialize, 0, toDeserialize.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);
                T obj = (T)bF.Deserialize(memoryStream);
                return obj;
            }
        }

    }
}
