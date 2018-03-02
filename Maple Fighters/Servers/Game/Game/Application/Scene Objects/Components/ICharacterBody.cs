using ComponentModel.Common;
using Game.Common;

namespace Game.Application.SceneObjects.Components
{
    internal interface ICharacterBody : IExposableComponent
    {
        PlayerState PlayerState { set; }
    }
}