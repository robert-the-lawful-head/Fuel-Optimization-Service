using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBOLinx.Web.Utilities.Extensions
{
    static class ListExtensions
    {
        public static IList<T> Clone<T>(this IList<T> listToClone)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<IList<T>>(Newtonsoft.Json.JsonConvert.SerializeObject(listToClone));
        }
    }
}
