using Game.Common;
using Scripts.Gameplay.Map;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class SpawnCharacterUtils
    {
        private const string GameObjectsPath = "Game/{0}";

        public static GameObject Create(Transform parent, CharacterClasses characterClass)
        {
            // Loading the character
            var path = string.Format(GameObjectsPath, characterClass);
            var characterObject = Resources.Load<GameObject>(path);

            // Creating the character
            var spawnedCharacter = 
                Object.Instantiate(characterObject, Vector3.zero, Quaternion.identity, parent);

            // Sets the position
            spawnedCharacter.transform.localPosition = characterObject.transform.localPosition;
            spawnedCharacter.transform.SetAsFirstSibling();

            // Sets the character name
            spawnedCharacter.name = spawnedCharacter.name.RemoveCloneFromName();

            return spawnedCharacter;
        }
    }
}