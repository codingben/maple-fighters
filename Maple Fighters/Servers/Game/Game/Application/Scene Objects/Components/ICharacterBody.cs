using ComponentModel.Common;
using Shared.Game.Common;

namespace Game.Application.SceneObjects.Components
{
    internal interface ICharacterBody : IExposableComponent
    {
        PlayerState PlayerState { set; }
    }
}