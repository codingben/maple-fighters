using System;
using UnityEngine;

namespace Scripts.Services.AuthenticatorApi
{
    [Serializable]
    public class RegistrationData
    {
        public string email;

        public string password;

        public string firstname;

        public string lastname;

        public override string ToString()
        {
            return JsonUtility.ToJson(this, true);
        }
    }
}