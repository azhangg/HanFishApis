using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Utils
{
    public static class StreamHelper
    {
        public static byte[]? SerializeObject(object obj)
        {
            if (obj is null)
                return null;
            return JsonSerializer.SerializeToUtf8Bytes(obj);
        }
        public static object? DeserializeObject(byte[] bytes)
        {
            object obj = null;
            if (bytes == null)
                return obj;
            MemoryStream ms = new MemoryStream(bytes);
            ms.Position = 0;
            var result = JsonSerializer.Deserialize(ms, typeof(object));
            ms.Close();
            return result;
        }   
    }
}
