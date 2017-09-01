using System.IO;
using CommonCommunicationInterfaces;
using CommunicationHelper;

namespace Shared.Game.Common
{
    public struct EntitiesRemovedEventParameters : IParameters
    {
        public int[] EntitiesId;

        public EntitiesRemovedEventParameters(int[] entity)
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