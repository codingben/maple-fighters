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

        public SceneObjectParameters(int id, string name, float x, float y)
        {
            Id = id;
            Name = name;
            X = x;
            Y = y;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(Name);
            writer.Write(X);
            writer.Write(Y);
        }

        public void Deserialize(BinaryReader reader)
        {
            Id = reader.ReadInt32();
            Name = reader.ReadString();
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
        }
    }
}