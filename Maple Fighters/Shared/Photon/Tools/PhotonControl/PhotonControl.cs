using System;
using System.Drawing;
using System.Windows.Forms;
using ComponentModel.Common;

namespace PhotonControl
{
    internal partial class PhotonControl : Form, IPhotonControl
    {
        private const string TITLE = "Photon Control";

        public IContainer<IPhotonControl> Components { get; }

        public ToolStripMenuItem ServersMenu { get; }
        public NotifyIcon NotifyIcon { get; }

        public event Action LogsFolderButtonClicked;
        public event Action ExitButtonClicked;

        public PhotonControl()
        {
            InitializeComponent();

            ServersMenu = serversMenuItem;

            NotifyIcon = notifyIcon;
            NotifyIcon.Icon = Properties.Resources.CircleIcon;
            NotifyIcon.ContextMenuStrip = menuStrip;
            NotifyIcon.Visible = true;

            Components = new Container<IPhotonControl>(this);
            Components.AddComponent(new ServersController());
            Components.AddComponent(new ServersCreator());
            Components.AddComponent(new ExitButtonHandler());
            Components.AddComponent(new LogsFolderButtonHandler());

            SubscribeToProcessExitEvent();
        }

        private void SubscribeToProcessExitEvent()
        {
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        }

        private void UnsubscribeFromProcessExitEvent()
        {
            AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            Hide();
            CreateMenuTitle();
        }

        private void OnExitButtonClicked(object sender, EventArgs e)
        {
            ExitButtonClicked?.Invoke();
        }

        private void OnLogsButtonClicked(object sender, EventArgs e)
        {
            LogsFolderButtonClicked?.Invoke();
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            Components.Dispose();

            UnsubscribeFromProcessExitEvent();
        }

        private void CreateMenuTitle()
        {
            var contextMenuStrip = new ContextMenuStrip();
            var title = contextMenuStrip.Items.Add(TITLE);
            title.BackColor = Color.Black;
            title.ForeColor = Color.White;
            title.Image = NotifyIcon.Icon.ToBitmap();

            const int FIRST_INDEX = 0;
            menuStrip.Items.Insert(FIRST_INDEX, title);
        }
    }
} 