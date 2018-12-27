namespace Scripts.Gameplay.Actors
{
    public interface IPlayerStateBehaviour
    {
        void OnStateEnter();

        void OnStateUpdate();

        void OnStateFixedUpdate();

        void OnStateExit();
    }
}