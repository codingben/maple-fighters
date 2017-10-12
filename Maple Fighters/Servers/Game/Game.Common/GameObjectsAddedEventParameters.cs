using System.IO;
using CommonCommunicationInterfaces;
using CommunicationHelper;

namespace Shared.Game.Common
{
    public struct GameObjectsAddedEventParameters : IParameters
    {
        public GameObject[] GameObjects;
        public CharacterInformation[] CharacterInformations;

        public GameObjectsAddedEventParameters(GameObject[] entity, CharacterInformation[] characterInformation)
        {
            GameObjects = entity;
            CharacterInformations = characterInformation;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.WriteArray(GameObjects);
            writer.WriteArray(CharacterInformations);
        }

        public void Deserialize(BinaryReader reader)
        {
            GameObjects = reader.ReadArray<GameObject>();
            CharacterInformations = reader.ReadArray<CharacterInformation>();
        }
    }
}