using Game.Common;
using Scripts.Containers;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    // TODO: Another name
    [RequireComponent(typeof(CharacterCreationNotifier))]
    public class CharacterCreator : MonoBehaviour
    {
        private CharacterCreationNotifier characterCreationNotifier;

        private void Awake()
        {
            characterCreationNotifier =
                GetComponent<CharacterCreationNotifier>();
        }

        private void Start()
        {
            if (characterCreationNotifier != null)
            {
                characterCreationNotifier.CreateCharacter += OnCreateCharacter;
            }
        }

        private void OnDestroy()
        {
            if (characterCreationNotifier != null)
            {
                characterCreationNotifier.CreateCharacter -= OnCreateCharacter;
            }
        }

        private void OnCreateCharacter(
            CharacterSpawnDetailsParameters characterSpawnDetails)
        {
            var id = characterSpawnDetails.SceneObjectId;
            var sceneObject = SceneObjectsContainer.GetInstance().GetRemoteSceneObject(id);
            if (sceneObject != null)
            {
                // TODO: Implement
            }
        }
    }
}