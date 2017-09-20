using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct EnterWorldOperationResponseParameters : IParameters
    {
        public Entity Entity;

        public EnterWorldOperationResponseParameters(Entity entity)
        {
            Entity = entity;
        }

        public void Serialize(BinaryWriter writer)
        {
            Entity.Serialize(writer);
        }

        public void Deserialize(BinaryReader reader)
        {
            Entity.Deserialize(reader);
        }
    }
}