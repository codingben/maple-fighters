using System.IO;
using CommonCommunicationInterfaces;

namespace ServerCommunication.Common
{
    public struct AuthenticateRequestParameters : IParameters
    {
        public string SecretKey;

        public AuthenticateRequestParameters(string secretKey)
        {
            SecretKey = secretKey;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(SecretKey);
        }

        public void Deserialize(BinaryReader reader)
        {
            SecretKey = reader.ReadString();
        }
    }
}