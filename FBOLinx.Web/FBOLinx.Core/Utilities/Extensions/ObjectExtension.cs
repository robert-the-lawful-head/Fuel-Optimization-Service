using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FBOLinx.Core.Utilities.Extensions
{
    public static class ObjectExtension
    {
        public static T Replicate<T>(this object obj)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(obj, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
        }
    }
}
