using Scripts.Containers;
using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class RemoteGameObjectPositionController : MonoBehaviour
    {
        private void Start()
        {
            ServiceContainer.GameService.PositionChanged.AddListener(OnPositionChanged);
        }

        private void OnPositionChanged(SceneObjectPositionChangedEventParameters parameters)
        {
            var id = parameters.SceneObjectId;
            var gameObject = SceneObjectsContainer.Instance.GetRemoteGameObject(id);
            gameObject?.GetGameObject()?.GetComponent<IPositionSetter>().SetPosition(new Vector2(parameters.X, parameters.Y), parameters.Direction);
        }
    }
}