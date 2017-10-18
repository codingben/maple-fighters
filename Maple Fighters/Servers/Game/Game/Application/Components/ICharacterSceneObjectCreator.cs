using ComponentModel.Common;
using Game.InterestManagement;
using Shared.Game.Common;

namespace Game.Application.Components
{
    internal interface ICharacterSceneObjectCreator : IExposableComponent
    {
        ISceneObject Create(Character character);
    }
}