using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class SpawnedCharacterCreator : MonoBehaviour, ISpawnedCharacterCreator
    {
        private const string GameObjectsPath = "Game/{0}";

        public GameObject Create(Transform parent, CharacterClasses characterClass)
        {
            // Loading the character
            var path = string.Format(GameObjectsPath, characterClass);
            var characterObject = Resources.Load<GameObject>(path);

            // Creating the character
            var spawnedCharacter = 
                Instantiate(characterObject, Vector3.zero, Quaternion.identity, parent);

            // Sets the position
            spawnedCharacter.transform.localPosition = characterObject.transform.localPosition;
            spawnedCharacter.transform.SetAsFirstSibling();

            // Sets the character name
            spawnedCharacter.name = spawnedCharacter.name.RemoveCloneFromName();

            return spawnedCharacter;
        }
    }
}