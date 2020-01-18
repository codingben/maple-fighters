namespace ServerCommon.Application.Components
{
    public interface IClientPeerContainer
    {
        void Add(int id, IPeerWrapper peer);

        void Remove(int id);

        bool Get(int id, out IPeerWrapper peer);
    }
}