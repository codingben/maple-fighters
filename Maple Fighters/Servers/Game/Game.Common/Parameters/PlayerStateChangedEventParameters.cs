using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct PlayerStateChangedEventParameters : IParameters
    {
        public PlayerState PlayerState;
        public int SceneObjectId;

        public PlayerStateChangedEventParameters(PlayerState playerState, int sceneObjectId)
        {
            PlayerState = playerState;
            SceneObjectId = sceneObjectId;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)PlayerState);
            writer.Write(SceneObjectId);
        }

        public void Deserialize(BinaryReader reader)
        {
            PlayerState = (PlayerState)reader.ReadByte();
            SceneObjectId = reader.ReadInt32();
        }
    }
}