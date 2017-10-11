using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct Character : IParameters
    {
        public CharacterClasses CharacterType;
        public string Name;
        public CharacterIndex Index;
        public bool HasCharacter;

        public Character(CharacterClasses characterType, string name, CharacterIndex characterIndex)
        {
            CharacterType = characterType;
            Name = name;
            Index = characterIndex;
            HasCharacter = true;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(HasCharacter);
            writer.Write((byte)Index);

            if (HasCharacter)
            {
                writer.Write((byte)CharacterType);
                writer.Write(Name);
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            HasCharacter = reader.ReadBoolean();
            Index = (CharacterIndex)reader.ReadByte();

            if (HasCharacter)
            {
                CharacterType = (CharacterClasses)reader.ReadByte();
                Name = reader.ReadString();
            }
        }
    }
}