using System.Collections;
using System.Linq;
using Game.Messages;
using ScriptableObjects.Configurations;
using Scripts.Services;
using UnityEngine;

namespace Scripts.Gameplay.Map.Dummy
{
    public class DummyEntitiesCreator : MonoBehaviour
    {
        [SerializeField]
        private DummyEntity[] dummyEntities;

        private void Start()
        {
            var networkConfiguration = NetworkConfiguration.GetInstance();
            if (networkConfiguration != null)
            {
                if (networkConfiguration.IsEditor())
                {
                    StartCoroutine(WaitFrameAndStart());
                }
            }
        }

        private IEnumerator WaitFrameAndStart()
        {
            yield return null;

            CreateDummyEntities();
        }

        private void CreateDummyEntities()
        {
            var gameApi = ApiProvider.ProvideGameApi();

            gameApi.GameObjectsAdded.Invoke(new GameObjectsAddedMessage()
            {
                GameObjects = dummyEntities.Select(x => new GameObjectData()
                {
                    Id = x.Id,
                    Name = x.Type.ToString(),
                    X = x.Position.x,
                    Y = x.Position.y,
                    Direction = 0,
                    CharacterName = string.Empty,
                    CharacterClass = 0,
                    HasCharacter = false
                }).ToArray(),
            });
        }
    }
}