using System.Collections.Generic;

namespace Scripts.UI.Controllers
{
    public interface IGameServerSelectorListener
    {
        void CreateGameServerViews(IEnumerable<UIGameServerButtonData> datas);
    }
}