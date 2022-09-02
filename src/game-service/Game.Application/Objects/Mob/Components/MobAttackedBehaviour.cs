using System.Timers;
using Game.Application.Components;

namespace Game.Application.Objects.Components
{
    public class MobAttackedBehaviour : ComponentBase, IMobAttackedBehaviour
    {
        private readonly Timer attackedTimer;

        public MobAttackedBehaviour()
        {
            attackedTimer = new Timer(2500);
            attackedTimer.Elapsed += (s, e) => OnAttackedTimeOver();
        }

        public void Start()
        {
            attackedTimer.Start();
        }

        public void Stop()
        {
            attackedTimer.Stop();
        }

        private void OnAttackedTimeOver()
        {
            var mobBehaviourManager = Components.Get<IMobBehaviourManager>();
            mobBehaviourManager.ChangeBehaviour(type: MobBehaviourType.Move);
        }
    }
}