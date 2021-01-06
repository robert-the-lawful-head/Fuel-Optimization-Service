using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace FBOLinx.Core.Utilities.Http
{
    public class JsonContent : StringContent
    {
        public JsonContent(object obj) :
            base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
        { }
    }
}
