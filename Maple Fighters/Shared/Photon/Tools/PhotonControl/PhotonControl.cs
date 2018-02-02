using System;
using System.Drawing;
using System.Windows.Forms;
using ComponentModel.Common;

namespace PhotonControl
{
    internal partial class PhotonControl : Form, IComponent, IPhotonControl
    {
        private const string TITLE = "Photon Control";

        public ToolStripMenuItem ServersMenu { get; private set; }
        public NotifyIcon NotifyIcon { get; private set; }

        public event Action LogsFolderButtonClicked;
        public event Action ExitButtonClicked;

        public PhotonControl()
        {
            InitializeComponent();
        }

        public void Awake(IContainer entity)
        {
            // Left blank intentionally
        }

        private void Form_Load(object sender, EventArgs e)
        {
            ServersMenu = serversStripMenuItem;

            NotifyIcon = notifyIcon;
            NotifyIcon.ContextMenuStrip = contextMenuStrip;

            Hide();
            CreateMenuTitle();
        }

        private void CreateMenuTitle()
        {
            var ContextMenuStrip = new ContextMenuStrip();
            var title = ContextMenuStrip.Items.Add(TITLE);
            title.BackColor = Color.Black;
            title.ForeColor = Color.White;

            contextMenuStrip.Items.Insert(0, title);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExitButtonClicked?.Invoke();
        }

        private void logsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogsFolderButtonClicked?.Invoke();
        }
    }
} 