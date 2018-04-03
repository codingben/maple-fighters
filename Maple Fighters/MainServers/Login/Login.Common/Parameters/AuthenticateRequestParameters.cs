using System.IO;
using CommonCommunicationInterfaces;

namespace Login.Common
{
    public struct AuthenticateRequestParameters : IParameters
    {
        public string Email;
        public string Password;

        public AuthenticateRequestParameters(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Email);
            writer.Write(Password);
        }

        public void Deserialize(BinaryReader reader)
        {
            Email = reader.ReadString();
            Password = reader.ReadString();
        }
    }
}