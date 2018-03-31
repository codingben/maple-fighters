using ComponentModel.Common;
using Game.Common;

namespace Game.Application.SceneObjects
{
    internal interface IPortalInfoProvider : IExposableComponent
    {
        Maps Map { get; }
    }
}