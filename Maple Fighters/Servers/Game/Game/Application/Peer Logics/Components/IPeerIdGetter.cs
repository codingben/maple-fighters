using ComponentModel.Common;

namespace Game.Application.PeerLogic.Components
{
    internal interface IPeerIdGetter : IExposableComponent
    {
        int GetId();
    }
}