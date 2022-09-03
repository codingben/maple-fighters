using System.Collections.Generic;
using Game.Application.Components;

namespace Game.Application.Objects.Components
{
    public class MobBehaviourManager : ComponentBase, IMobBehaviourManager
    {
        private readonly Dictionary<MobBehaviourType, IMobBehaviour> behaviours = new();
        private IMobBehaviour behaviour;

        protected override void OnAwake()
        {
            var mobMoveBehaviour = Components.Get<IMobMoveBehaviour>();
            var mobAttackedBehaviour = Components.Get<IMobAttackedBehaviour>();
            behaviours.Add(MobBehaviourType.Move, mobMoveBehaviour);
            behaviours.Add(MobBehaviourType.Attacked, mobAttackedBehaviour);

            var presenceSceneProvider = Components.Get<IPresenceSceneProvider>();
            presenceSceneProvider.SceneChanged += OnSceneInitialized;
        }

        private void OnSceneInitialized(IGameScene gameScene)
        {
            ChangeBehaviour(type: MobBehaviourType.Move);
        }

        public void ChangeBehaviour(MobBehaviourType type)
        {
            StopPreviousBehaviour();

            if (behaviours.TryGetValue(type, out behaviour))
            {
                behaviour.Start();
            }
        }

        private void StopPreviousBehaviour()
        {
            behaviour?.Stop();
        }
    }
}