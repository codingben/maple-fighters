using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct UpdatePositionRequestParameters : IParameters
    {
        public float X;
        public float Y;
        public Directions Direction;

        public UpdatePositionRequestParameters(float x, float y, Directions direction)
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