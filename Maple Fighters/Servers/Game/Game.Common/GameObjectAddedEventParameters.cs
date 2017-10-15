using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct GameObjectAddedEventParameters : IParameters
    {
        public GameObject GameObject;
        public CharacterInformation CharacterInformation;
        public bool HasCharacter;

        public GameObjectAddedEventParameters(GameObject gameObject, CharacterInformation characterInformation, bool hasCharacter)
        {
            GameObject = gameObject;
            CharacterInformation = characterInformation;
            HasCharacter = hasCharacter;
        }

        public void Serialize(BinaryWriter writer)
        {
            GameObject.Serialize(writer);

            writer.Write(HasCharacter);

            if (HasCharacter)
            {
                CharacterInformation.Serialize(writer);
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            GameObject.Deserialize(reader);

            HasCharacter = reader.ReadBoolean();

            if (HasCharacter)
            {
                CharacterInformation.Deserialize(reader);
            }
        }
    }
}