using Common.ComponentModel;

namespace ServerCommon.Application.Components
{
    public interface IServerComponentsProvider
    {
        void AddCommonComponents();

        void RemoveCommonComponents();

        IComponentsProvider GetComponentsProvider();
    }
}