using ComponentModel.Common;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Components
{
    internal interface ICharacterGetter : IExposableComponent
    {
        Character GetCharacter();
    }
}