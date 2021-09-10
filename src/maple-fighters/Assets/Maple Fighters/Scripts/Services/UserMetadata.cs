using UnityEngine;
using Scripts.Services.AuthenticatorApi;

namespace Scripts.Services
{
    public class UserMetadata : MonoBehaviour
    {
        public UserData UserData { get; set; }

        public string GameServerUrl { get; set; }
        
        public int CharacterType { get; set; }
        
        public string CharacterName { get; set; }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}