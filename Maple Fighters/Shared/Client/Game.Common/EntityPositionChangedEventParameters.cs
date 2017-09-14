using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct EntityPositionChangedEventParameters : IParameters
    {
        public int EntityId;
        public float X;
        public float Y;
        public Directions Direction;

        public EntityPositionChangedEventParameters(int entityId, float x, float y, Directions direction)
        {
            EntityId = entityId;
            X = x;
            Y = y;
            Direction = direction;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(EntityId);
            writer.Write(X);
            writer.Write(Y);
            writer.Write((byte)Direction);
        }

        public void Deserialize(BinaryReader reader)
        {
            EntityId = reader.ReadInt32();
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            Direction = (Directions)reader.ReadByte();
        }
    }
}