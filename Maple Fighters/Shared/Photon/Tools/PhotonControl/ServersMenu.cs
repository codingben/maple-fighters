using System.Windows.Forms;
using CommonTools.Log;
using ComponentModel.Common;

namespace PhotonControl
{
    internal class ServersMenu : Component, IServersMenu
    {
        private IPhotonControl photonControl;
        private IServersController serversController;

        protected override void OnAwake()
        {
            base.OnAwake();

            photonControl = Entity.GetComponent<IPhotonControl>().AssertNotNull();
            serversController = Entity.GetComponent<IServersController>().AssertNotNull("Servers controller is null");
        }

        public ToolStripMenuItem AddServerItemToServersMenu(string serverName)
        {
            if (string.IsNullOrEmpty(serverName) || string.IsNullOrWhiteSpace(serverName))
            {
                LogUtils.Log(@"The server name incorrect. Please check it.");
                return null;
            }

            var serverItem = new ToolStripMenuItem(serverName)
            {
                Name = serverName
            };

            photonControl.ServersMenu.DropDownItems.Add(serverItem);

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
                LogUtils.Log(@"The server name incorrect. Please check it.");
                return;
            }

            photonControl.ServersMenu.DropDownItems.RemoveByKey(serverName);
        }
    }
}