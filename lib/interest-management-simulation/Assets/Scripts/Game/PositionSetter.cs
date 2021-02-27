using UnityEngine;

namespace Game.InterestManagement.Simulation
{
    [RequireComponent(typeof(SceneObject))]
    public class PositionSetter : MonoBehaviour
    {
        private IGameObject sceneObject;
        private Vector3 position;

        private void Awake()
        {
            sceneObject = GetComponent<IGameObject>();
        }

        public void Update()
        {
            if (position != transform.position)
            {
                position = transform.position;

                sceneObject.Transform.SetPosition(position.ToVector2());
            }
        }
    }
}