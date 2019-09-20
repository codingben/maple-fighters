using System.Collections.Generic;

namespace Scripts.UI.GameServerBrowser
{
    public interface IOnGameServerReceivedListener
    {
        void OnGameServerReceived(IEnumerable<UIGameServerButtonData> datas);
    }
}