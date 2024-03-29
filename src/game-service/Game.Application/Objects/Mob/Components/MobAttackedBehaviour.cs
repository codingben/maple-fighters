using System.Timers;
using Game.Application.Components;

namespace Game.Application.Objects.Components
{
    public class MobAttackedBehaviour : ComponentBase, IMobAttackedBehaviour
    {
        private readonly Timer attackedTimer;

        public MobAttackedBehaviour()
        {
            attackedTimer = new Timer(2000);
            attackedTimer.Elapsed += (s, e) => OnAttackedTimeOver();
        }

        public void Start()
        {
            attackedTimer.Stop();
            attackedTimer.Start();
        }

        public void Stop()
        {
            attackedTimer.Stop();
        }

        private void OnAttackedTimeOver()
        {
            var behaviourManager = Components.Get<IMobBehaviourManager>();
            behaviourManager.ChangeBehaviour(type: MobBehaviourType.Move);
        }
    }
}