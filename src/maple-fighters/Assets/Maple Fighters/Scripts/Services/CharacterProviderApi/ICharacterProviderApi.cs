using System;

namespace Scripts.Services.CharacterProviderApi
{
    public interface ICharacterProviderApi
    {
        Action<long, string> CreateCharacterCallback { get; set; }

        Action<long, string> DeleteCharacterCallback { get; set; }

        Action<long, string> GetCharactersCallback { get; set; }

        void CreateCharacter(int userid, string charactername, int index, int classindex);

        void DeleteCharacter(int characterid);

        void GetCharacters(int userid);
    }
}