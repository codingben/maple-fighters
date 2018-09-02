using Common.ComponentModel;

namespace ServerCommon.Application.Components
{
    public class ServerComponentsProvider : IServerComponentsProvider
    {
        private readonly IComponentsProvider components;

        public ServerComponentsProvider()
        {
            components = new ComponentsProvider();
        }

        public void AddCommonComponents()
        {
            components.Add(new IdGenerator());
            components.Add(new RandomNumberGenerator());
        }

        public void RemoveCommonComponents()
        {
            components.Remove<IdGenerator>();
            components.Remove<RandomNumberGenerator>();
        }

        public IComponentsProvider GetComponentsProvider()
        {
            return components;
        }
    }
}