using System.IO;
using CommonCommunicationInterfaces;

namespace Game.Common
{
    public struct CharacterSpawnDetailsParameters : IParameters
    {
        public int SceneObjectId;
        public CharacterParameters Character;
        public Directions Direction;

        public CharacterSpawnDetailsParameters(int sceneObjectId, CharacterParameters character, Directions direction)
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