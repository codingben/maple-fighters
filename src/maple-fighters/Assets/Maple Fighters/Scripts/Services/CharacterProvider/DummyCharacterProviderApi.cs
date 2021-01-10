using UnityEngine;

namespace Scripts.Services.CharacterProviderApi
{
    public class DummyCharacterProviderApi : MonoBehaviour, ICharacterProviderApi
    {
        public static DummyCharacterProviderApi GetInstance()
        {
            if (instance == null)
            {
                var characterProviderApi =
                    new GameObject("Dummy CharacterProvider Api");
                instance =
                    characterProviderApi.AddComponent<DummyCharacterProviderApi>();
            }

            return instance;
        }

        private static DummyCharacterProviderApi instance;
    }
}