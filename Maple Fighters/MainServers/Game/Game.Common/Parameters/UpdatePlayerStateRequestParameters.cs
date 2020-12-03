using System.IO;
using CommonCommunicationInterfaces;

namespace Game.Common
{
    public struct UpdatePlayerStateRequestParameters : IParameters
    {
        public PlayerState PlayerState;

        public UpdatePlayerStateRequestParameters(PlayerState playerState)
        {
            PlayerState = playerState;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)PlayerState);
        }

        public void Deserialize(BinaryReader reader)
        {
            PlayerState = (PlayerState)reader.ReadByte();
        }
    }
}