using UnityEngine;

namespace Game.MessageTools
{
    public class UnityJsonSerializer : IJsonSerializer
    {
        public string Serialize<T>(T data)
            where T : struct
        {
            return JsonUtility.ToJson(data);
        }

        public T Deserialize<T>(string json)
            where T : struct
        {
            return JsonUtility.FromJson<T>(json);
        }
    }
}