namespace Scripts.Gameplay.Actors
{
    public interface IPlayerStateBehaviour
    {
        void OnStateEnter(IPlayerController playerController);
        void OnStateUpdate();
        void OnStateFixedUpdate();
        void OnStateExit();
    }
}