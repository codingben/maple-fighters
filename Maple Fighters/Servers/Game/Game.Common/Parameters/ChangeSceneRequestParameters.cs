using System.IO;
using CommonCommunicationInterfaces;

namespace Game.Common
{
    public struct ChangeSceneRequestParameters : IParameters
    {
        public int PortalId;

        public ChangeSceneRequestParameters(int portalId)
        {
            PortalId = portalId;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(PortalId);
        }

        public void Deserialize(BinaryReader reader)
        {
            PortalId = reader.ReadInt32();
        }
    }
}