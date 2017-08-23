using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct EntityPositionChangedEventParameters : IParameters
    {
        public int EntityId;
        public float X;
        public float Y;

        public EntityPositionChangedEventParameters(int entityId, float x, float y)
        {
            EntityId = entityId;
            X = x;
            Y = y;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(EntityId);
            writer.Write(X);
            writer.Write(Y);
        }

        public void Deserialize(BinaryReader reader)
        {
            EntityId = reader.ReadInt32();
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
        }
    }
}