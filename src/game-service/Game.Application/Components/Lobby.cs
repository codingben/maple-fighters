using System.Collections.Generic;
using Common.MathematicsHelper;
using Game.Application.Objects;

namespace Game.Application.Components
{
    public class Lobby
    {
        public IEnumerable<IGameObject> CreateGameObjects()
        {
            yield return CreateGuardian();
            yield return CreatePortal();
        }

        private IGameObject CreateGuardian()
        {
            var id = 0;
            var position = new Vector2(-14.24f, -2.025f);
            var guardian = new GuardianGameObject(id);

            guardian.Transform.SetPosition(position);
            guardian.Transform.SetSize(Vector2.One);

            // guardian.AddProximityChecker(MatrixRegion);
            guardian.AddBubbleNotification("Hello", 1);

            return guardian;
        }

        private IGameObject CreatePortal()
        {
            var id = 1;
            var position = new Vector2(-17.125f, -1.5f);
            var portal = new PortalGameObject(id);

            // portal.AddProximityChecker(MatrixRegion);
            portal.AddPortalData((byte)Map.TheDarkForest);

            return portal;
        }
    }
}