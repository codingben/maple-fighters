using System;
using System.Windows.Forms;
using System.Xml;
using CommonTools.Log;
using ComponentModel.Common;

namespace PhotonControl
{
    internal class ServersCreator : Component
    {
        private const string PHOTON_SERVER_CONFIGURATION_FILE = @"{0}\PhotonServer.config";
        private const int MINIMUM_SERVERS_TO_ADD_START_OR_STOP_SERVERS_BUTTONS = 2;

        private IPhotonControl photonControl;
        private IServersContainer serversContainer;
        private IServersController serversController;

        protected override void OnAwake()
        {
            base.OnAwake();

            photonControl = Entity.GetComponent<IPhotonControl>().AssertNotNull();
            serversContainer = Entity.GetComponent<IServersContainer>().AssertNotNull();
            serversController = Entity.GetComponent<IServersController>().AssertNotNull();

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
                    LogUtils.Log(@"Could not find server names.");
                }
            }
            catch (Exception exception)
            {
                LogUtils.Log(exception.Message);
            }
            finally
            {
                var serversCount = serversContainer.GetNumberOfServers();
                if (serversCount == 0)
                {
                    var info = new ToolStripMenuItem("No servers.");
                    photonControl.ServersMenu.DropDownItems.Insert(0, info);

                    var message = "No servers were found.";
                    photonControl.NotifyIcon.ShowBalloonTip(100, null, message, ToolTipIcon.Warning);
                }
                else
                {
                    if (serversCount >= MINIMUM_SERVERS_TO_ADD_START_OR_STOP_SERVERS_BUTTONS)
                    {
                        AddStartAndStopServersButtons();
                    }

                    var message = $"{serversCount} servers were found.";
                    photonControl.NotifyIcon.ShowBalloonTip(100, null, message, ToolTipIcon.Warning);
                }
            }

            void AddStartAndStopServersButtons()
            {
                var startServers = new ToolStripMenuItem("Start Servers");
                startServers.Click += (o, args) => serversController.StartAllServers();

                var stopServers = new ToolStripMenuItem("Stop Servers");
                stopServers.Click += (o, args) => serversController.StopAllServers();

                photonControl.ServersMenu.DropDownItems.Insert(0, startServers);
                photonControl.ServersMenu.DropDownItems.Insert(1, stopServers);
                photonControl.ServersMenu.DropDownItems.Insert(2, new ToolStripSeparator());
            }
        }
    }
}