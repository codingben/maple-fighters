using System.IO;
using CommonCommunicationInterfaces;

namespace Authorization.Client.Common
{
    public struct AuthorizeRequestParameters : IParameters
    {
        public string AccessToken;

        public AuthorizeRequestParameters(string accessToken)
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