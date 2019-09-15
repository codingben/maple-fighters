namespace Scripts.Gameplay.Player
{
    public interface IPlayerStateBehaviour
    {
        void OnStateEnter();

        void OnStateUpdate();

        void OnStateFixedUpdate();

        void OnStateExit();
    }
}