using Game.Application.Objects.Components;
using InterestManagement;

namespace Game.Application.Objects
{
    public class PlayerGameObject : GameObject
    {
        public PlayerGameObject(int id)
            : base(id, name: "Player")
        {
            Components.Add(new GameObjectGetter(this));
            Components.Add(new AnimationData());
            // Components.Add(new MessageSender(SendMessageToMySession, SendMessageToSession));
            Components.Add(new PositionChangedMessageSender());
            Components.Add(new AnimationStateChangedMessageSender());
            Components.Add(new CharacterData());
        }

        public void AddProximityChecker(IMatrixRegion<IGameObject> matrixRegion)
        {
            var proximityChecker = Components.Add(new ProximityChecker());
            proximityChecker.SetMatrixRegion(matrixRegion);
        }
    }
}