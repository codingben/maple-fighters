using System;
using System.Windows.Forms;
using System.Xml;
using CommonTools.Log;
using ComponentModel.Common;

namespace PhotonControl
{
    internal class ServersCreator : Component<IPhotonControl>
    {
        private const string PHOTON_SERVER_CONFIGURATION_FILE = @"{0}\PhotonServer.config";
        private const int MINIMUM_SERVERS_TO_ADD_START_OR_STOP_SERVERS_BUTTONS = 2;

        private IServersContainer serversContainer;
        private IServersController serversController;

        protected override void OnAwake()
        {
            base.OnAwake();

            serversContainer = Entity.Components.GetComponent<IServersContainer>().AssertNotNull();
            serversController = Entity.Components.GetComponent<IServersController>().AssertNotNull();

            LoadServerNamesFromPhotonConfiguration();
        }

        private void LoadServerNamesFromPhotonConfiguration()
        {
            try
            {
                var photonConfiguration = new XmlDocument();
                photonConfiguration.Load(string.Format(PHOTON_SERVER_CONFIGURATION_FILE, Application.StartupPath));

                var node = photonConfiguration.SelectSingleNode("Configuration");
                if (node != null)
                {
                    foreach (XmlElement xmlElement in node)
                    {
                        serversContainer.AddServer(xmlElement.Name);
                    }
                }
                else
                {
                    LogUtils.Log(MessageBuilder.Trace(@"Could not find server names."));
                }
            }
            catch (Exception exception)
            {
                LogUtils.Log(MessageBuilder.Trace(exception.Message));
            }
            finally
            {
                var serversCount = serversContainer.GetNumberOfServers();
                if (serversCount == 0)
                {
                    const int FIRST_INDEX = 0;

                    var info = new ToolStripMenuItem("No servers.");
                    Entity.ServersMenu.DropDownItems.Insert(FIRST_INDEX, info);

                    var message = "No servers were found.";
                    Entity.NotifyIcon.ShowBalloonTip(100, null, message, ToolTipIcon.Info);
                }
                else
                {
                    if (serversCount >= MINIMUM_SERVERS_TO_ADD_START_OR_STOP_SERVERS_BUTTONS)
                    {
                        AddStartAndStopServersButtons();
                    }

                    var message = $"{serversCount} servers were found.";
                    Entity.NotifyIcon.ShowBalloonTip(100, null, message, ToolTipIcon.Info);
                }

                ChangeNotifyIconInfo();
            }

            void AddStartAndStopServersButtons()
            {
                const string START_BUTTON_NAME = "Start Servers";
                const string STOP_BUTTON_NAME = "Stop Servers";

                var startServers = new ToolStripMenuItem(START_BUTTON_NAME);
                startServers.Click += (o, args) => serversController.StartAllServers();

                var stopServers = new ToolStripMenuItem(STOP_BUTTON_NAME);
                stopServers.Click += (o, args) => serversController.StopAllServers();

                const int FIRST_INDEX = 0;
                const int SECOND_INDEX = 1;
                const int THIRD_INDEX = 2;

                Entity.ServersMenu.DropDownItems.Insert(FIRST_INDEX, startServers);
                Entity.ServersMenu.DropDownItems.Insert(SECOND_INDEX, stopServers);
                Entity.ServersMenu.DropDownItems.Insert(THIRD_INDEX, new ToolStripSeparator());
            }
        }

        private void ChangeNotifyIconInfo()
        {
            var count = serversContainer.GetNumberOfServers();
            switch (count)
            {
                case 0:
                {
                    Entity.NotifyIcon.Text = @"There are no servers available.";
                    break;
                }
                case 1:
                {
                    Entity.NotifyIcon.Text = @"There is 1 server available.";
                    break;
                }
                default:
                {
                    Entity.NotifyIcon.Text = $@"There are {count} servers available.";
                    break;
                }
            }
        }
    }
}