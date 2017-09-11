using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct Entity : IParameters
    {
        public int Id;
        public EntityType Type;
        public float X;
        public float Y;

        public Entity(int id, EntityType type, float x, float y)
        {
            Id = id;
            Type = type;
            X = x;
            Y = y;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write((byte)Type);
            writer.Write(X);
            writer.Write(Y);
        }

        public void Deserialize(BinaryReader reader)
        {
            Id = reader.ReadInt32();
            Type = (EntityType)reader.ReadByte();
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
        }
    }
}