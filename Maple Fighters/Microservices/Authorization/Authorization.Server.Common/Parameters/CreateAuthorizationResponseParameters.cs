using System.IO;
using CommonCommunicationInterfaces;

namespace Authorization.Server.Common
{
    public struct CreateAuthorizationResponseParameters : IParameters
    {
        public string AccessToken;

        public CreateAuthorizationResponseParameters(string accessToken)
        {
            AccessToken = accessToken;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(AccessToken);
        }

        public void Deserialize(BinaryReader reader)
        {
            AccessToken = reader.ReadString();
        }
    }
}