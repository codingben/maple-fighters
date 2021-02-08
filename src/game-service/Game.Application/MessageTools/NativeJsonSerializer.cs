using System.Text.Json;

namespace Game.MessageTools
{
    public class NativeJsonSerializer : IJsonSerializer
    {
        public string Serialize<T>(T data)
            where T : struct
        {
            return JsonSerializer.Serialize(data);
        }

        public T Deserialize<T>(string json)
            where T : struct
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}