using UnityEngine;

namespace Scripts.Services.AuthenticatorApi
{
    public struct LoginData
    {
        public string email;

        public string password;

        public override string ToString()
        {
            return JsonUtility.ToJson(this, true);
        }
    }
}