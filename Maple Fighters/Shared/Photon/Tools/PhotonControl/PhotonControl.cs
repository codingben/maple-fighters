using System;
using System.Drawing;
using System.Windows.Forms;
using ComponentModel.Common;
using PhotonControl.Components;

namespace PhotonControl
{
    internal partial class PhotonControl : Form, IPhotonControl
    {
        private const string TITLE = "Photon Control";

        public IContainer<IPhotonControl> Components { get; private set; }

        public ToolStripMenuItem ServersMenu { get; private set; }
        public NotifyIcon NotifyIcon { get; private set; }

        public event Action ClearLogsButtonClicked;
        public event Action LogsFolderButtonClicked;
        public event Action ExitButtonClicked;

        public PhotonControl()
        {
            InitializeComponent();
            InitializeGUI();

            AddComponents();

            SubscribeToProcessExitEvent();
        }

        private void InitializeGUI()
        {
            ServersMenu = serversMenuItem;
            NotifyIcon = notifyIcon;
            NotifyIcon.Icon = Properties.Resources.CircleIcon;
            NotifyIcon.ContextMenuStrip = menuStrip;
            NotifyIcon.Visible = true;
        }

        private void AddComponents()
        {
            Components = new Container<IPhotonControl>(this);
            Components.AddComponent(new ServersController());
            Components.AddComponent(new ServersCreator());
            Components.AddComponent(new ExitButtonHandler());
            Components.AddComponent(new LogsFolderButtonHandler());
            Components.AddComponent(new ClearLogsButtonHandler());
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

        private void OnClearLogsButtonClicked(object sender, EventArgs e)
        {
            ClearLogsButtonClicked?.Invoke();
        }

        private void OnLogsFolderButtonClicked(object sender, EventArgs e)
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