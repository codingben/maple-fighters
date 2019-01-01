using CommonTools.Log;
using Game.Common;
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
            DummyPortalContainer.GetInstance().Add(sceneObject.Id, map);
        }
    }
}