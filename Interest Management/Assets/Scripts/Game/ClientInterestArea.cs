using UnityEngine;

namespace Game.InterestManagement.Simulation
{
    [RequireComponent(typeof(SceneObject))]
    public class ClientInterestArea : MonoBehaviour
    {
        private IInterestArea<IGameObject> interestArea;

        private void Start()
        {
            var scene = SceneUtils.GetSceneGameObject();
            var gameObject = GetComponent<IGameObject>();
            interestArea = 
                new InterestArea<IGameObject>(scene.GetScene(), gameObject);
        }

        public INearbySceneObjectsEvents<IGameObject> GetNearbySceneObjectsEvents()
        {
            return interestArea.NearbySceneObjectsEvents;
        }
    }
}