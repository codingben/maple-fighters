using System.IO;
using CommonCommunicationInterfaces;

namespace Game.Common
{
    public struct CharacterParameters : IParameters
    {
        public string Name;
        public CharacterIndex Index;
        public CharacterClasses CharacterType;
        public Maps LastMap;
        public bool HasCharacter;

        public CharacterParameters(string name, CharacterClasses characterType, CharacterIndex characterIndex, Maps lastMap = Maps.Map_1)
        {
            Name = name;
            CharacterType = characterType;
            Index = characterIndex;
            HasCharacter = true;
            LastMap = lastMap;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(HasCharacter);
            writer.Write((byte)Index);

            if (HasCharacter)
            {
                writer.Write(Name);
                writer.Write((byte)CharacterType);
                writer.Write((byte)LastMap);
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
                LastMap = (Maps)reader.ReadByte();
            }
        }
    }
}