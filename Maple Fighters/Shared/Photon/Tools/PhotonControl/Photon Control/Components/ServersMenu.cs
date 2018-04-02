using System.Windows.Forms;
using CommonTools.Log;
using ComponentModel.Common;
using PhotonControl.Components.Interfaces;

namespace PhotonControl
{
    internal class ServersMenu : Component<IPhotonControl>, IServersMenu
    {
        private IServersController serversController;

        protected override void OnAwake()
        {
            base.OnAwake();

            serversController = Entity.Components.GetComponent<IServersController>().AssertNotNull();
        }

        public ToolStripMenuItem AddServerItemToServersMenu(string serverName)
        {
            if (string.IsNullOrEmpty(serverName) || string.IsNullOrWhiteSpace(serverName))
            {
                LogUtils.Log(MessageBuilder.Trace(@"The server name incorrect. Please check it."));
                return null;
            }

            var serverItem = new ToolStripMenuItem(serverName)
            {
                Name = serverName
            };

            Entity.ServersMenu.DropDownItems.Add(serverItem);

            AddCommandsItemsToServerMenu();
            return serverItem;

            void AddCommandsItemsToServerMenu()
            {
                const string START_BUTTON = "Start";
                const string STOP_BUTTON = "Stop";

                serverItem.DropDownItems.Add(START_BUTTON, null, (o, args) => serversController.StartServer(serverName));
                var stop = serverItem.DropDownItems.Add(STOP_BUTTON, null, (o, args) => serversController.StopServer(serverName));
                stop.Enabled = false;
            }
        }

        public void RemoveServerItemFromServersMenu(string serverName)
        {
            if (string.IsNullOrEmpty(serverName) || string.IsNullOrWhiteSpace(serverName))
            {
                LogUtils.Log(MessageBuilder.Trace(@"The server name incorrect. Please check it."));
                return;
            }

            Entity.ServersMenu.DropDownItems.RemoveByKey(serverName);
        }
    }
}