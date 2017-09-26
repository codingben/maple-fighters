using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct PlayerStateChangedEventParameters : IParameters
    {
        public PlayerState PlayerState;
        public int GameObjectId;

        public PlayerStateChangedEventParameters(PlayerState playerState, int gameObjectId)
        {
            PlayerState = playerState;
            GameObjectId = gameObjectId;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)PlayerState);
            writer.Write(GameObjectId);
        }

        public void Deserialize(BinaryReader reader)
        {
            PlayerState = (PlayerState)reader.ReadByte();
            GameObjectId = reader.ReadInt32();
        }
    }
}