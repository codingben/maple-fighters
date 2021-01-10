using UnityEngine;

namespace Scripts.Services.CharacterProviderApi
{
    public class HttpCharacterProviderApi : MonoBehaviour, ICharacterProviderApi
    {
        public static HttpCharacterProviderApi GetInstance()
        {
            if (instance == null)
            {
                var characterProviderApi =
                    new GameObject("Http CharacterProvider Api");
                instance =
                    characterProviderApi.AddComponent<HttpCharacterProviderApi>();
            }

            return instance;
        }

        private static HttpCharacterProviderApi instance;
    }
}