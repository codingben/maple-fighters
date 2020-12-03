using System.IO;
using CommonCommunicationInterfaces;

namespace UserProfile.Server.Common
{
    public struct CreateUserProfileResponseParameters : IParameters
    {
        public UserProfileCreationStatus Status;

        public CreateUserProfileResponseParameters(UserProfileCreationStatus status = UserProfileCreationStatus.Failed)
        {
            Status = status;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)Status);
        }

        public void Deserialize(BinaryReader reader)
        {
            Status = (UserProfileCreationStatus)reader.ReadByte();
        }
    }
}