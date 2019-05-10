using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace WebDavCore
{
    public static class Json
    {
        public static string Serialize<T>(T obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);

                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        public static T Deserialize<T>(byte[] json)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T), new DataContractJsonSerializerSettings
            {
                UseSimpleDictionaryFormat = true,
            });

            using (MemoryStream ms = new MemoryStream(json))
            {
                var obj = (T)serializer.ReadObject(ms);

                return obj;
            }
        }

        public static T Deserialize<T>(string json) => Deserialize<T>(Encoding.UTF8.GetBytes(json));
    }
}
