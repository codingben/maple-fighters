using System.IO;
using CommonCommunicationInterfaces;
using CommunicationHelper;

namespace Shared.Game.Common
{
    public struct EntityRemovedEventParameters : IParameters
    {
        public Entity[] Entity;

        public EntityRemovedEventParameters(Entity[] entity)
        {
            Entity = entity;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.WriteArray(Entity);
        }

        public void Deserialize(BinaryReader reader)
        {
            Entity = reader.ReadArray<Entity>();
        }
    }
}