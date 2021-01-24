using System;
using UnityEngine;

namespace Scripts.Services.AuthenticatorApi
{
    [Serializable]
    public class LoginData
    {
        public string email;

        public string password;

        public override string ToString()
        {
            return JsonUtility.ToJson(this, true);
        }
    }
}