using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct CharacterInformation : IParameters
    {
        public string CharacterName;
        public CharacterClasses CharacterClass;

        public CharacterInformation(string characterName, CharacterClasses characterClass)
        {
            CharacterName = characterName;
            CharacterClass = characterClass;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(CharacterName);
            writer.Write((byte)CharacterClass);
        }

        public void Deserialize(BinaryReader reader)
        {
            CharacterName = reader.ReadString();
            CharacterClass = (CharacterClasses)reader.ReadByte();
        }
    }
}