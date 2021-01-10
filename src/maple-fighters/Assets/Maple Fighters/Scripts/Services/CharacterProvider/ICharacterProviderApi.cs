using System;

namespace Scripts.Services.CharacterProviderApi
{
    public interface ICharacterProviderApi
    {
        Action<long, byte> CreateCharacterCallback { get; set; }

        Action<long, byte> DeleteCharacterCallback { get; set; }

        Action<long, byte> GetCharactersCallback { get; set; }

        void CreateCharacter(int userid, string charactername, int index, int classindex);

        void DeleteCharacter(int characterid);

        void GetCharacters(int userid);
    }
}