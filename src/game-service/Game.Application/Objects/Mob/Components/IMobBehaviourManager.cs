namespace Game.Application.Objects.Components
{
    public interface IMobBehaviourManager
    {
        void ChangeBehaviour(MobBehaviourType type);

        MobBehaviourType GetBehaviour();
    }
}