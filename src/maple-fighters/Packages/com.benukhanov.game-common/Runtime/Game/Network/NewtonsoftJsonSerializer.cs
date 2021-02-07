using Newtonsoft.Json;

namespace Game.Network
{
    public class NewtonsoftJsonSerializer : IJsonSerializer
    {
        public string Serialize<T>(T data)
            where T : class
        {
            return JsonConvert.SerializeObject(data);
        }

        public T Deserialize<T>(string json)
            where T : class
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}