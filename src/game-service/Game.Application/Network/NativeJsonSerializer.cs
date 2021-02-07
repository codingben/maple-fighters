using System.Text.Json;

namespace Game.Network
{
    public class NativeJsonSerializer : IJsonSerializer
    {
        public string Serialize<T>(T data)
            where T : class
        {
            return JsonSerializer.Serialize(data);
        }

        public T Deserialize<T>(string json)
            where T : class
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}