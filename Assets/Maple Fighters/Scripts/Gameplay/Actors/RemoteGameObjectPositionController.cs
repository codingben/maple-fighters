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

        private void OnPositionChanged(GameObjectPositionChangedEventParameters parameters)
        {
            var id = parameters.GameObjectId;
            var gameObject = GameObjectsContainer.Instance.GetRemoteGameObject(id);
            gameObject?.GetGameObject()?.GetComponent<IPositionSetter>().SetPosition(new Vector2(parameters.X, parameters.Y), parameters.Direction);
        }
    }
}