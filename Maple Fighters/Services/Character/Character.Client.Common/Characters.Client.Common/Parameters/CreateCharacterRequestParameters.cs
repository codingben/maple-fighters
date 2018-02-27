using System.IO;
using CommonCommunicationInterfaces;

namespace Character.Client.Common
{
    public struct CreateCharacterRequestParameters : IParameters
    {
        public CharacterClasses CharacterClass;
        public string Name;
        public CharacterIndex Index;

        public CreateCharacterRequestParameters(CharacterClasses characterClass, string name, CharacterIndex characterIndex)
        {
            CharacterClass = characterClass;
            Name = name;
            Index = characterIndex;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)CharacterClass);
            writer.Write(Name);
            writer.Write((byte)Index);
        }

        public void Deserialize(BinaryReader reader)
        {
            CharacterClass = (CharacterClasses)reader.ReadByte();
            Name = reader.ReadString();
            Index = (CharacterIndex)reader.ReadByte();
        }
    }
}