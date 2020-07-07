using System.Collections.Generic;
using Common.MathematicsHelper;
using Game.Application.Objects;

namespace Game.Application.Components
{
    public class TheDarkForest
    {
        public IEnumerable<IGameObject> CreateGameObjects()
        {
            yield return CreatePortal();
            yield return CreateBlueSnail();
        }

        private IGameObject CreatePortal()
        {
            var id = 0;
            var position = new Vector2(12.5f, -1.125f);
            var portal = new PortalGameObject(id);

            // portal.AddProximityChecker(MatrixRegion);
            portal.AddPortalData((byte)Map.Lobby);

            return portal;
        }

        private IGameObject CreateBlueSnail()
        {
            var id = 1;
            var position = new Vector2(-2f, -8.2f);
            var blueSnail = new BlueSnailGameObject(id);

            // blueSnail.AddProximityChecker(MatrixRegion);

            return blueSnail;
        }
    }
}