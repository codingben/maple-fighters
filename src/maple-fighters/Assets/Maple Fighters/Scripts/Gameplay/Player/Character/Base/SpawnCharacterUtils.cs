using Game.Common;
using Scripts.Constants;
using Scripts.Gameplay.Map;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    public static class SpawnCharacterUtils
    {
        public static GameObject Create(Transform parent, CharacterClasses characterClass)
        {
            // Loading the character
            var path = 
                string.Format(Paths.Resources.GameObjectsPath, characterClass);
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