using ComponentModel.Common;
using Game.InterestManagement;

namespace Game.Application.PeerLogic.Components
{
    internal interface ISceneObjectGetter : IExposableComponent
    {
        ISceneObject GetSceneObject();
    }
}