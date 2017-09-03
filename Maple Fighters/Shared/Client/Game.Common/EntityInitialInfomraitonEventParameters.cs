using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct EntityInitialInfomraitonEventParameters : IParameters
    {
        public Entity Entity;
        public float X;
        public float Y;

        public EntityInitialInfomraitonEventParameters(Entity entity, float x, float y)
        {
            Entity = entity;
            X = x;
            Y = y;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);

            Entity.Serialize(writer);
        }

        public void Deserialize(BinaryReader reader)
        {
            X = reader.ReadSingle();
            Y = reader.ReadSingle();

            Entity.Deserialize(reader);
        }
    }
}