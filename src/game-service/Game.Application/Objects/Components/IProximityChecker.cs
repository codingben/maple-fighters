using System.Collections.Generic;

namespace Game.Application.Objects.Components
{
    public interface IProximityChecker
    {
        IEnumerable<IGameObject> GetNearbyGameObjects();
    }
}