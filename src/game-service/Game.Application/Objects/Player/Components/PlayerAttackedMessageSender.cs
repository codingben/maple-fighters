using System.Timers;
using Common.ComponentModel;
using Game.Messages;

namespace Game.Application.Objects.Components
{
    public class PlayerAttackedMessageSender : ComponentBase, IPlayerAttacked
    {
        private IMessageSender messageSender;
        private bool isAttacked;

        protected override void OnAwake()
        {
            messageSender = Components.Get<IMessageSender>();
        }

        public void Attack(int attackerId)
        {
            if (isAttacked)
            {
                return;
            }

            isAttacked = true;

            WaitToBeAttackableAgain(seconds: 500);

            SendAttackedMessage(attackerId);
        }

        void WaitToBeAttackableAgain(int seconds)
        {
            var timer = new Timer();
            timer.Elapsed += (s, e) =>
            {
                isAttacked = false;
                timer?.Dispose();
            };
            timer.Interval = seconds;
            timer.Start();
        }

        private void SendAttackedMessage(int attackerId)
        {
            var messageCode = (byte)MessageCodes.Attacked;
            var message = new AttackedMessage()
            {
                AttackerId = attackerId
            };

            messageSender.SendMessage(messageCode, message);
        }
    }
}