using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct GameObjectAddedEventParameters : IParameters
    {
        public GameObject GameObject;
        public CharacterInformation? CharacterInformation;

        public GameObjectAddedEventParameters(GameObject gameObject, CharacterInformation? characterInformation)
        {
            GameObject = gameObject;
            CharacterInformation = characterInformation;
        }

        public void Serialize(BinaryWriter writer)
        {
            GameObject.Serialize(writer);
            CharacterInformation?.Serialize(writer);
        }

        public void Deserialize(BinaryReader reader)
        {
            GameObject.Deserialize(reader);
            CharacterInformation?.Deserialize(reader);
        }
    }
}