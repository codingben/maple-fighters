using System.IO;
using CommonCommunicationInterfaces;

namespace Game.Common
{
    public struct SceneObjectPositionChangedEventParameters : IParameters
    {
        public int SceneObjectId;
        public float X;
        public float Y;
        public Directions Direction;

        public SceneObjectPositionChangedEventParameters(int sceneObjectId, float x, float y, Directions direction)
        {
            SceneObjectId = sceneObjectId;
            X = x;
            Y = y;
            Direction = direction;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(SceneObjectId);
            writer.Write(X);
            writer.Write(Y);
            writer.Write((byte)Direction);
        }

        public void Deserialize(BinaryReader reader)
        {
            SceneObjectId = reader.ReadInt32();
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            Direction = (Directions)reader.ReadByte();
        }
    }
}