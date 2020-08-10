using System.Collections;
using System.Linq;
using Game.Messages;
using Scripts.Services.Game;
using UnityEngine;

namespace Scripts.Gameplay.Map.Dummy
{
    public class DummyEntitiesCreator : MonoBehaviour
    {
        [SerializeField]
        private DummyEntity[] dummyEntities;

        private void Start()
        {
            StartCoroutine(WaitFrameAndStart());
        }

        private IEnumerator WaitFrameAndStart()
        {
            yield return null;

            CreateDummyEntities();
        }

        private void CreateDummyEntities()
        {
            var gameApi = FindObjectOfType<GameApi>();

            gameApi.GameObjectsAdded.Invoke(new GameObjectsAddedMessage()
            {
                GameObjects = dummyEntities.Select(x => new GameObjectData()
                {
                    Id = x.Id,
                    Name = x.Type.ToString(),
                    X = x.Position.x,
                    Y = x.Position.y,
                    CharacterData = new CharacterData(),
                    HasCharacter = false
                }).ToArray(),
            });
        }
    }
}