using System.IO;
using CommonCommunicationInterfaces;

namespace Authorization.Server.Common
{
    public struct AuthorizeAccesTokenRequestParameters : IParameters
    {
        public string AccessToken;

        public AuthorizeAccesTokenRequestParameters(string accessToken)
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