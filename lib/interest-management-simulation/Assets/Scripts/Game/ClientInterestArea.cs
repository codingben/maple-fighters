using InterestManagement;
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
            var matrixRegion = scene.GetScene().MatrixRegion;
            var gameObject = GetComponent<IGameObject>();

            interestArea = new InterestArea<IGameObject>(gameObject, new UnityLogger());
            interestArea.SetMatrixRegion(matrixRegion);
        }

        private void OnDestroy()
        {
            interestArea?.Dispose();
        }

        public INearbySceneObjectsEvents<IGameObject> GetNearbySceneObjectsEvents()
        {
            return interestArea.GetNearbySceneObjectsEvents();
        }
    }
}