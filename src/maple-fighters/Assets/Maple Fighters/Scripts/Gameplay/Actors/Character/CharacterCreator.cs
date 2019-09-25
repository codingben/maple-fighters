using System.Collections;
using Game.Common;
using Scripts.Gameplay.Entity;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
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
            StartCoroutine(WaitFrameAndSpawn(characterSpawnDetails));
        }

        // TODO: Hack
        private IEnumerator WaitFrameAndSpawn(CharacterSpawnDetailsParameters characterSpawnDetails)
        {
            yield return null;

            var id = characterSpawnDetails.SceneObjectId;
            var entity = EntityContainer.GetInstance().GetRemoteEntity(id);
            if (entity != null)
            {
                var spawnedCharacterDetails = 
                    entity.GameObject.GetComponent<SpawnedCharacterDetails>();
                var spawnedCharacter = 
                    entity.GameObject.GetComponent<SpawnCharacter>();
                spawnedCharacterDetails.SetCharacterDetails(characterSpawnDetails);
                spawnedCharacter.Spawn();
            }
        }
    }
}