using ComponentModel.Common;
using Shared.Game.Common;

namespace Game.Application.SceneObjects
{
    internal interface IPortalInfoProvider : IExposableComponent
    {
        Maps Map { get; }
    }
}