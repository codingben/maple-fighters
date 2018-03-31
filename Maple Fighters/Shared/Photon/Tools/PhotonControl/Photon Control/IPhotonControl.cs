using System;
using ComponentModel.Common;

namespace PhotonControl
{
    internal interface IPhotonControl : IDisposable, IEntity, IPhotonControlGUI, IPhotonControlEvents
    {
        IContainer<IPhotonControl> Components { get; }
    }
}