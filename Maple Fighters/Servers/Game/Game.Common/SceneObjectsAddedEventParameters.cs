using System.IO;
using CommonCommunicationInterfaces;
using CommunicationHelper;

namespace Shared.Game.Common
{
    public struct SceneObjectsAddedEventParameters : IParameters
    {
        public SceneObject[] SceneObjects;
        public CharacterInformation[] CharacterInformations;

        public SceneObjectsAddedEventParameters(SceneObject[] entity, CharacterInformation[] characterInformation)
        {
            SceneObjects = entity;
            CharacterInformations = characterInformation;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.WriteArray(SceneObjects);
            writer.WriteArray(CharacterInformations);
        }

        public void Deserialize(BinaryReader reader)
        {
            SceneObjects = reader.ReadArray<SceneObject>();
            CharacterInformations = reader.ReadArray<CharacterInformation>();
        }
    }
}