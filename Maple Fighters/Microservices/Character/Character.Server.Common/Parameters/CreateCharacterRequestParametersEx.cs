using System.IO;
using CommonCommunicationInterfaces;
using Game.Common;

namespace Character.Server.Common
{
    public struct CreateCharacterRequestParametersEx : IParameters
    {
        public int UserId;
        public CharacterClasses CharacterClass;
        public string Name;
        public CharacterIndex Index;

        public CreateCharacterRequestParametersEx(int userId, CharacterClasses characterClass, string name, CharacterIndex characterIndex)
        {
            UserId = userId;
            CharacterClass = characterClass;
            Name = name;
            Index = characterIndex;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(UserId);
            writer.Write((byte)CharacterClass);
            writer.Write(Name);
            writer.Write((byte)Index);
        }

        public void Deserialize(BinaryReader reader)
        {
            UserId = reader.ReadInt32();
            CharacterClass = (CharacterClasses)reader.ReadByte();
            Name = reader.ReadString();
            Index = (CharacterIndex)reader.ReadByte();
        }
    }
}