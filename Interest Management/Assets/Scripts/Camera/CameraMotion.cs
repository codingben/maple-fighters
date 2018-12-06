using Assets.Scripts.Game.Utils;
using UnityEngine;

namespace Assets.Scripts.Camera
{
    public class CameraMotion : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        [SerializeField]
        private float speed = 5;

        private CameraBoundaries cameraBoundaries;

        private void Awake()
        {
            if (target == null)
            {
                Debug.LogWarning("There is no target for the camera.");
            }
        }

        private void Start()
        {
            var gameScene = SceneUtils.GetSceneGameObject();
            var size = new Vector2(
                gameScene.GetSceneSize().x / 2,
                gameScene.GetSceneSize().y / 2);
            cameraBoundaries = new CameraBoundaries(
                upperBound: size,
                lowerBound: size * -1);
        }

        private void LateUpdate()
        {
            if (target != null)
            {
                Move();
            }
        }

        private void Move()
        {
            var destination = cameraBoundaries.Transform(
                new Vector3(
                    target.position.x,
                    target.position.y,
                    transform.position.z));
            transform.position = Vector3.Lerp(
                transform.position,
                destination,
                speed * Time.deltaTime);
        }
    }
}