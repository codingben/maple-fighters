using UnityEngine;

namespace Game.InterestManagement.Simulation
{
    [RequireComponent(typeof(SceneObject))]
    public class PositionSetter : MonoBehaviour
    {
        private IGameObject sceneObject;

        private void Awake()
        {
            sceneObject = GetComponent<IGameObject>();
        }

        public void Update()
        {
            var vector2 = transform.position.ToVector2();
            sceneObject.Transform.SetPosition(vector2);
        }
    }
}