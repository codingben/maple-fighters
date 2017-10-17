using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct SceneObject : IParameters
    {
        public int Id;
        public string Name;
        public float X;
        public float Y;

        public SceneObject(int id, string name, float x, float y)
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