using UnityEngine;

namespace Game.MessageTools
{
    public class UnityJsonSerializer : IJsonSerializer
    {
        public string Serialize<T>(T data)
            where T : class
        {
            return JsonUtility.ToJson(data);
        }

        public T Deserialize<T>(string json)
            where T : class
        {
            return JsonUtility.FromJson<T>(json);
        }
    }
}