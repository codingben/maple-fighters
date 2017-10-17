using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct CharacterInformation : IParameters
    {
        public int SceneObjectId;
        public string CharacterName;
        public CharacterClasses CharacterClass;

        public CharacterInformation(int sceneObjectId, string characterName, CharacterClasses characterClass)
        {
            SceneObjectId = sceneObjectId;
            CharacterName = characterName;
            CharacterClass = characterClass;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(SceneObjectId);
            writer.Write(CharacterName);
            writer.Write((byte)CharacterClass);
        }

        public void Deserialize(BinaryReader reader)
        {
            SceneObjectId = reader.ReadInt32();
            CharacterName = reader.ReadString();
            CharacterClass = (CharacterClasses)reader.ReadByte();
        }
    }
}