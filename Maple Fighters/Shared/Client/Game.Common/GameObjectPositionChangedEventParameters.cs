using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct GameObjectPositionChangedEventParameters : IParameters
    {
        public int GameObjectId;
        public float X;
        public float Y;
        public Directions Direction;

        public GameObjectPositionChangedEventParameters(int gameObjectId, float x, float y, Directions direction)
        {
            GameObjectId = gameObjectId;
            X = x;
            Y = y;
            Direction = direction;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(GameObjectId);
            writer.Write(X);
            writer.Write(Y);
            writer.Write((byte)Direction);
        }

        public void Deserialize(BinaryReader reader)
        {
            GameObjectId = reader.ReadInt32();
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            Direction = (Directions)reader.ReadByte();
        }
    }
}