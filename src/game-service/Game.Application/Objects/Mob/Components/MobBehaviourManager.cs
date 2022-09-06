using System.Collections.Generic;
using Game.Application.Components;

namespace Game.Application.Objects.Components
{
    public class MobBehaviourManager : ComponentBase, IMobBehaviourManager
    {
        private readonly Dictionary<MobBehaviourType, IMobBehaviour> behaviours = new();
        private IMobBehaviour behaviour;
        private MobBehaviourType behaviourType;

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
                behaviourType = type;
                behaviour.Start();
            }
        }

        public MobBehaviourType GetBehaviour()
        {
            return behaviourType;
        }

        private void StopPreviousBehaviour()
        {
            behaviour?.Stop();
        }
    }
}