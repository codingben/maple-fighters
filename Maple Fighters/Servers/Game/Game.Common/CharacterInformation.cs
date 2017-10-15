using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct CharacterInformation : IParameters
    {
        public int GameObjectId;
        public string CharacterName;
        public CharacterClasses CharacterClass;

        public CharacterInformation(int gameObjectId, string characterName, CharacterClasses characterClass)
        {
            GameObjectId = gameObjectId;
            CharacterName = characterName;
            CharacterClass = characterClass;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(GameObjectId);
            writer.Write(CharacterName);
            writer.Write((byte)CharacterClass);
        }

        public void Deserialize(BinaryReader reader)
        {
            GameObjectId = reader.ReadInt32();
            CharacterName = reader.ReadString();
            CharacterClass = (CharacterClasses)reader.ReadByte();
        }
    }
}