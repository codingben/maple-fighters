using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct EnterWorldOperationResponseParameters : IParameters
    {
        public Entity Entity;
        public float X;
        public float Y;

        public EnterWorldOperationResponseParameters(Entity entity, float x, float y)
        {
            Entity = entity;
            X = x;
            Y = y;
        }

        public void Serialize(BinaryWriter writer)
        {
            Entity.Serialize(writer);

            writer.Write(X);
            writer.Write(Y);
        }

        public void Deserialize(BinaryReader reader)
        {
            Entity.Deserialize(reader);

            X = reader.ReadSingle();
            Y = reader.ReadSingle();
        }
    }
}