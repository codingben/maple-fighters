using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct UpdateEntityPositionRequestParameters : IParameters
    {
        public float X;
        public float Y;

        public UpdateEntityPositionRequestParameters(float x, float y)
        {
            X = x;
            Y = y;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
        }

        public void Deserialize(BinaryReader reader)
        {
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
        }
    }
}