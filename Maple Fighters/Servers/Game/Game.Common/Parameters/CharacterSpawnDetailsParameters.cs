using System.IO;
using Character.Client.Common;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct CharacterSpawnDetailsParameters : IParameters
    {
        public int SceneObjectId;
        public CharacterFromDatabaseParameters Character;
        public Directions Direction;

        public CharacterSpawnDetailsParameters(int sceneObjectId, CharacterFromDatabaseParameters character, Directions direction)
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