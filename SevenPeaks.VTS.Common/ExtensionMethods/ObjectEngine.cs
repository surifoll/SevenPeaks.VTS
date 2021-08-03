using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace SevenPeaks.VTS.Common.ExtensionMethods
{
    public static class ObjectEngine
    {
        public static byte[] GetByteArray(this object obj)
        {
            return obj == null ? null : Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
        }
        
        public static object GetObject(this byte[] arr)
        {
            return arr == null ? null : JsonConvert.DeserializeObject(Encoding.UTF8.GetString(arr));
        } 
        public static string GetString(this byte[] arr)
        {
            return arr == null ? null : Encoding.UTF8.GetString(arr);
        }

        public static string GetQueryString(this List<KeyValuePair<string, StringValues>> keyValuePairs)
        {
            var val = keyValuePairs.Select(p => new KeyValueModel { Key = p.Key, Value = p.Value })
                .Where(p => p.Key != "pageNumber" && p.Key != "pageSize");

            string output = string.Join("&", val.Select(p => $"{p.Key}={System.Web.HttpUtility.UrlEncode(p.Value)}"));
            return output;
        }
    }
}