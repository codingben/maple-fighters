using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct PlayerAttackedEventParameters : IParameters
    {
        public float ContactPointX;
        public float ContactPointY;

        public PlayerAttackedEventParameters(float contactPointX, float contactPointY)
        {
            ContactPointX = contactPointX;
            ContactPointY = contactPointY;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(ContactPointX);
            writer.Write(ContactPointY);
        }

        public void Deserialize(BinaryReader reader)
        {
            ContactPointX = reader.ReadSingle();
            ContactPointY = reader.ReadSingle();
        }
    }
}