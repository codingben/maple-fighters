using System.IO;
using Authenticator.Common.Enums;
using CommonCommunicationInterfaces;

namespace Authenticator.Common.Parameters
{
    public struct LoginResponseParameters : IParameters
    {
        public LoginStatus LoginStatus;

        public LoginResponseParameters(LoginStatus loginStatus)
        {
            LoginStatus = loginStatus;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)LoginStatus);
        }

        public void Deserialize(BinaryReader reader)
        {
            LoginStatus = (LoginStatus)reader.ReadByte();
        }
    }
}