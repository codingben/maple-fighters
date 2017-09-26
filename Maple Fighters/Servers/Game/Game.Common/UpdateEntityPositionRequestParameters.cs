using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct UpdateEntityPositionRequestParameters : IParameters
    {
        public float X;
        public float Y;
        public Directions Direction;

        public UpdateEntityPositionRequestParameters(float x, float y, Directions direction)
        {
            X = x;
            Y = y;
            Direction = direction;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write((byte)Direction);
        }

        public void Deserialize(BinaryReader reader)
        {
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            Direction = (Directions)reader.ReadByte();
        }
    }
}