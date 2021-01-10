using System;
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