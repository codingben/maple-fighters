using System.IO;
using CommonCommunicationInterfaces;
using CommunicationHelper;

namespace Shared.Game.Common
{
    public struct EntitiesAddedEventParameters : IParameters
    {
        public Entity[] Entity;

        public EntitiesAddedEventParameters(Entity[] entity)
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