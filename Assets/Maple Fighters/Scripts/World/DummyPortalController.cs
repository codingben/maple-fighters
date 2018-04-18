using CommonTools.Log;
using Game.Common;
using Scripts.Containers;
using Scripts.Gameplay;
using Scripts.Services;
using UnityEngine;

namespace Scripts.World
{
    public class DummyPortalController : MonoBehaviour
    {
        public void CreateTeleportation(Maps map)
        {
            var sceneObject = GetComponent<ISceneObject>().AssertNotNull();
            var gameScenePeerLogic = ServiceContainer.GameService.GetPeerLogic<IDummyGameScenePeerLogicAPI>().AssertNotNull();
            var portalContainer = gameScenePeerLogic.Components.GetComponent<IPortalContainer>().AssertNotNull();
            portalContainer.Add(sceneObject.Id, map);
        }
    }
}