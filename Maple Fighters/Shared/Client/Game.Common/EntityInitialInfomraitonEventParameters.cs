using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct EntityInitialInfomraitonEventParameters : IParameters
    {
        public Entity Entity;

        public EntityInitialInfomraitonEventParameters(Entity entity)
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