using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct Entity : IParameters
    {
        public int Id;
        public EntityType Type;

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write((byte)Type);
        }

        public void Deserialize(BinaryReader reader)
        {
            Id = reader.ReadInt32();
            Type = (EntityType)reader.ReadByte();
        }
    }
}