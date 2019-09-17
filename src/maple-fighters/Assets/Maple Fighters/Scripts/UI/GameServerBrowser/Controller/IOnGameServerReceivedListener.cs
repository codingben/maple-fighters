using System.Collections.Generic;

namespace Scripts.UI.Controllers
{
    public interface IOnGameServerReceivedListener
    {
        void OnGameServerReceived(IEnumerable<UIGameServerButtonData> datas);
    }
}