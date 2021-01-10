using System;
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

        public Action<long, byte> CreateCharacterCallback { get; set; }

        public Action<long, byte> DeleteCharacterCallback { get; set; }

        public Action<long, byte> GetCharactersCallback { get; set; }

        public void CreateCharacter(int userid, string charactername, int index, int classindex)
        {
            throw new NotImplementedException();
        }

        public void DeleteCharacter(int characterid)
        {
            throw new NotImplementedException();
        }

        public void GetCharacters(int userid)
        {
            throw new NotImplementedException();
        }
    }
}