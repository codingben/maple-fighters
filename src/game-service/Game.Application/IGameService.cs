namespace Game.Application
{
    public interface IGameService
    {
        void SendMessage(byte[] data, int id);
    }
}