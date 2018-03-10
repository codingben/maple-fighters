using System.IO;
using CommonCommunicationInterfaces;

namespace Character.Server.Common
{
    public struct GetCharactersRequestParameters : IParameters
    {
        public int UserId;

        public GetCharactersRequestParameters(int userId)
        {
            UserId = userId;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(UserId);
        }

        public void Deserialize(BinaryReader reader)
        {
            UserId = reader.ReadInt32();
        }
    }
}