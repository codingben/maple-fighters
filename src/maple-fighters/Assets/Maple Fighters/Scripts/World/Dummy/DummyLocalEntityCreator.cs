using System.Collections;
using Scripts.Gameplay.Actors;
using Scripts.Network.Services;
using UnityEngine;

namespace Scripts.World.Dummy
{
    [RequireComponent(typeof(DummyCharacterDetailsProvider))]
    public class DummyLocalEntityCreator : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(WaitFrameAndStart());
        }

        private IEnumerator WaitFrameAndStart()
        {
            yield return null;

            CreateLocalDummyEntity();
        }

        private void CreateLocalDummyEntity()
        {
            var dummyCharacterDetailsProvider =
                GetComponent<DummyCharacterDetailsProvider>();
            var parameters =
                dummyCharacterDetailsProvider.GetDummyCharacterParameters();
            var gameSceneApi = ServiceProvider.GameService.GetGameSceneApi();
            gameSceneApi?.SceneEntered.Invoke(parameters);
        }
    }
}