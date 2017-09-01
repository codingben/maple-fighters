using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct EntityRemovedEventParameters : IParameters
    {
        public int EntityId;

        public EntityRemovedEventParameters(int entity)
        {
            EntityId = entity;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(EntityId);
        }

        public void Deserialize(BinaryReader reader)
        {
            EntityId = reader.ReadInt32();
        }
    }
}