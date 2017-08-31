using System.IO;
using CommonCommunicationInterfaces;
using CommunicationHelper;

namespace Shared.Game.Common
{
    public struct EntityRemovedEventParameters : IParameters
    {
        public int[] EntitiesId;

        public EntityRemovedEventParameters(int[] entity)
        {
            EntitiesId = entity;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.WriteInt32Array(EntitiesId);
        }

        public void Deserialize(BinaryReader reader)
        {
            EntitiesId = reader.ReadInt32Array();
        }
    }
}