namespace Game.Application.Objects.Components
{
    public interface IBubbleNotifier
    {
        void Notify(int notifierId, string text, int time);
    }
}