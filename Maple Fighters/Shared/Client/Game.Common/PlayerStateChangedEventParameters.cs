using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct PlayerStateChangedEventParameters : IParameters
    {
        public PlayerState PlayerState;
        public int EntityId;

        public PlayerStateChangedEventParameters(PlayerState playerState, int entityId)
        {
            PlayerState = playerState;
            EntityId = entityId;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)PlayerState);
            writer.Write(EntityId);
        }

        public void Deserialize(BinaryReader reader)
        {
            PlayerState = (PlayerState)reader.ReadByte();
            EntityId = reader.ReadInt32();
        }
    }
}