using UnityEngine;

namespace Scripts.Services.AuthenticatorApi
{
    public struct RegistrationData
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