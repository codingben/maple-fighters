using ComponentModel.Common;

namespace Scripts.Services
{
    public interface IDummyGameScenePeerLogicAPI : IPeerLogicBase
    {
        IContainer Components { get; }
    }
}