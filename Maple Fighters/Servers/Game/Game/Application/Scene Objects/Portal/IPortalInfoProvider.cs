using ComponentModel.Common;
using MathematicsHelper;
using Shared.Game.Common;

namespace Game.Application.SceneObjects
{
    internal interface IPortalInfoProvider : IExposableComponent
    {
        Vector2 PlayerPosition { get; }
        Maps Map { get; }
    }
}