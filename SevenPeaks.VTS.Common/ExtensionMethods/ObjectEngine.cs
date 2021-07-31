using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SevenPeaks.VTS.Common.ExtensionMethods
{
    public static class ObjectEngine
    {
        public static byte[] GetByteArray(this object obj)
        {
            return obj == null ? null : Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
        }
        
        public static object GetString(this byte[] arr)
        {
            return arr == null ? null : JsonConvert.DeserializeObject(Encoding.UTF8.GetString(arr));
        }
    }
}