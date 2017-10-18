using ComponentModel.Common;
using Game.InterestManagement;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Components
{
    internal interface ICharacterSceneObjectGetter : IExposableComponent
    {
        ISceneObject GetSceneObject();
        Character GetCharacter();
    }
}