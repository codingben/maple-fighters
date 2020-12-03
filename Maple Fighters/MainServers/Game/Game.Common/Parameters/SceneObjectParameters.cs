using System.IO;
using CommonCommunicationInterfaces;

namespace Game.Common
{
    public struct SceneObjectParameters : IParameters
    {
        public int Id;
        public string Name;
        public float X;
        public float Y;
        public Directions Direction;

        public SceneObjectParameters(int id, string name, float x, float y, Directions direction)
        {
            Id = id;
            Name = name;
            X = x;
            Y = y;
            Direction = direction;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(Name);
            writer.Write(X);
            writer.Write(Y);
            writer.Write((byte)Direction);
        }

        public void Deserialize(BinaryReader reader)
        {
            Id = reader.ReadInt32();
            Name = reader.ReadString();
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            Direction = (Directions)reader.ReadByte();
        }
    }
}