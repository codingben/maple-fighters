using System.IO;
using CommonCommunicationInterfaces;

namespace Chat.Common
{
    public struct AuthenticateRequestParameters : IParameters
    {
        public string AccessToken;

        public AuthenticateRequestParameters(string accessToken)
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