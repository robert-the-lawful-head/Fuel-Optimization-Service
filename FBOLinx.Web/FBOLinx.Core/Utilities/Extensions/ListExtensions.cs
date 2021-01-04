using System.Collections.Generic;

namespace FBOLinx.Core.Utilities.Extensions
{
    public static class ListExtensions
    {
        public static IList<T> Clone<T>(this IList<T> listToClone)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<IList<T>>(Newtonsoft.Json.JsonConvert.SerializeObject(listToClone));
        }
    }
}
