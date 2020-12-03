using ComponentModel.Common;

namespace PhotonControl
{
    internal interface IPhotonControl : IEntity<IPhotonControl>, IPhotonControlGUI, IPhotonControlEvents
    {
        // Left blank intentionally
    }
}