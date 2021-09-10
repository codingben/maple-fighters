using UnityEngine;

namespace Scripts.Services.AuthenticatorApi
{
    public struct UserData
    {
        public string id;

        public static UserData FromJson(string json)
        {
            return JsonUtility.FromJson<UserData>(json);
        }
    }
}