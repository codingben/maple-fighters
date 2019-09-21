using Game.Common;
using Scripts.Gameplay;
using UnityEngine;

namespace Scripts.World.Dummy
{
    [RequireComponent(typeof(ISceneObject))]
    public class DummyPortalController : MonoBehaviour
    {
        public void CreateTeleportation(Maps map)
        {
            var sceneObject = GetComponent<ISceneObject>();
            if (sceneObject != null)
            {
                DummyPortalContainer.GetInstance().Add(sceneObject.Id, map);
            }
        }
    }
}