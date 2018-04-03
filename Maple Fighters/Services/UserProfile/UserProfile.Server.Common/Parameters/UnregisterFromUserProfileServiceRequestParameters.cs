using System.IO;
using CommonCommunicationInterfaces;

namespace UserProfile.Server.Common
{
    public struct UnregisterFromUserProfileServiceRequestParameters : IParameters
    {
        public int ServerId;

        public UnregisterFromUserProfileServiceRequestParameters(int serverId)
        {
            ServerId = serverId;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(ServerId);
        }

        public void Deserialize(BinaryReader reader)
        {
            ServerId = reader.ReadInt32();
        }
    }
}