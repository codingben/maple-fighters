using UnityEngine;

namespace Game.InterestManagement.Simulation
{
    [RequireComponent(typeof(SceneObject))]
    public class ClientInterestAreaVisualGraphicsCreator : MonoBehaviour
    {
        private const string InterestAreaObject = "Interest Area";
        private IGameObject sceneObject;

        private void Start()
        {
            sceneObject = GetComponent<IGameObject>();

            CreateInterestAreaVisualGraphics();
        }

        private void CreateInterestAreaVisualGraphics()
        {
            var interestAreaGameObject = CreateInterestAreaGameObject();
            var tracker = interestAreaGameObject
                .GetComponent<ClientInterestAreaVisualGraphicsTracker>();
            tracker.SetTarget(transform);
        }

        private GameObject CreateInterestAreaGameObject()
        {
            var interestAreaObject = Resources.Load<GameObject>(InterestAreaObject);
            var interestAreaGameObject = Instantiate(
                interestAreaObject,
                sceneObject.Transform.Position.FromVector2(),
                Quaternion.identity);
            interestAreaGameObject.transform.localScale =
                sceneObject.Transform.Size.FromVector2();
            interestAreaGameObject.name = $"{name} (Interest Area)";

            return interestAreaGameObject;
        }
    }
}