using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct CharacterSpawnDetails : IParameters
    {
        public int SceneObjectId;
        public CharacterFromDatabase Character;
        public Directions Direction;

        public CharacterSpawnDetails(int sceneObjectId, CharacterFromDatabase character, Directions direction)
        {
            SceneObjectId = sceneObjectId;
            Character = character;
            Direction = direction;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(SceneObjectId);
            writer.Write((byte)Direction);

            Character.Serialize(writer);
        }

        public void Deserialize(BinaryReader reader)
        {
            SceneObjectId = reader.ReadInt32();
            Direction = (Directions)reader.ReadByte();

            Character.Deserialize(reader);
        }
    }
}