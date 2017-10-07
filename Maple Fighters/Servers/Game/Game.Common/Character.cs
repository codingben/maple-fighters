using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct Character : IParameters
    {
        public CharacterClasses CharacterType;
        public string Name;

        public Character(CharacterClasses characterType, string name)
        {
            CharacterType = characterType;
            Name = name;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)CharacterType);
            writer.Write(Name);
        }

        public void Deserialize(BinaryReader reader)
        {
            CharacterType = (CharacterClasses)reader.ReadByte();
            Name = reader.ReadString();
        }
    }
}