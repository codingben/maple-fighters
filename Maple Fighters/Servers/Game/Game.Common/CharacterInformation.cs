using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct CharacterInformation : IParameters
    {
        public int SceneObjectId;
        public string CharacterName;
        public CharacterClasses CharacterClass;
        public Directions Direction;

        public CharacterInformation(int sceneObjectId, string characterName, CharacterClasses characterClass, Directions direction)
        {
            SceneObjectId = sceneObjectId;
            CharacterName = characterName;
            CharacterClass = characterClass;
            Direction = direction;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(SceneObjectId);
            writer.Write(CharacterName);
            writer.Write((byte)CharacterClass);
            writer.Write((byte)Direction);
        }

        public void Deserialize(BinaryReader reader)
        {
            SceneObjectId = reader.ReadInt32();
            CharacterName = reader.ReadString();
            CharacterClass = (CharacterClasses)reader.ReadByte();
            Direction = (Directions)reader.ReadByte();
        }
    }
}