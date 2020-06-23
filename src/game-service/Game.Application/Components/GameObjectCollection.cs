using System;
using Common.ComponentModel;
using Game.Application.Objects;

namespace Game.Application.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class GameObjectCollection : ComponentBase, IGameObjectCollection
    {
        public GameObjectCollection()
        {

        }

        public bool TryGetGameObject(int id, out IGameObject gameObject)
        {
            throw new NotImplementedException();
        }
    }
}