using System.IO;
using CommonCommunicationInterfaces;

namespace Characters.Client.Common
{
    public struct CharacterFromDatabaseParameters : IParameters
    {
        public string Name;
        public CharacterIndex Index;
        public CharacterClasses CharacterType;
        public bool HasCharacter;

        public CharacterFromDatabaseParameters(string name, CharacterClasses characterType, CharacterIndex characterIndex)
        {
            Name = name;
            CharacterType = characterType;
            Index = characterIndex;
            HasCharacter = true;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(HasCharacter);
            writer.Write((byte)Index);

            if (HasCharacter)
            {
                writer.Write(Name);
                writer.Write((byte)CharacterType);
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            HasCharacter = reader.ReadBoolean();
            Index = (CharacterIndex)reader.ReadByte();

            if (HasCharacter)
            {
                Name = reader.ReadString();
                CharacterType = (CharacterClasses)reader.ReadByte();
            }
        }
    }
}