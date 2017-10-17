using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct SceneObjectAddedEventParameters : IParameters
    {
        public SceneObject SceneObject;
        public CharacterInformation CharacterInformation;
        public bool HasCharacter;

        public SceneObjectAddedEventParameters(SceneObject sceneObject, CharacterInformation characterInformation, bool hasCharacter)
        {
            SceneObject = sceneObject;
            CharacterInformation = characterInformation;
            HasCharacter = hasCharacter;
        }

        public void Serialize(BinaryWriter writer)
        {
            SceneObject.Serialize(writer);

            writer.Write(HasCharacter);

            if (HasCharacter)
            {
                CharacterInformation.Serialize(writer);
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            SceneObject.Deserialize(reader);

            HasCharacter = reader.ReadBoolean();

            if (HasCharacter)
            {
                CharacterInformation.Deserialize(reader);
            }
        }
    }
}