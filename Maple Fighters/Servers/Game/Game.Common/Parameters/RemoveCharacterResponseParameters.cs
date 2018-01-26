using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct RemoveCharacterResponseParameters : IParameters
    {
        public RemoveCharacterStatus Status;

        public RemoveCharacterResponseParameters(RemoveCharacterStatus status)
        {
            Status = status;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)Status);
        }

        public void Deserialize(BinaryReader reader)
        {
            Status = (RemoveCharacterStatus)reader.ReadByte();
        }
    }
}