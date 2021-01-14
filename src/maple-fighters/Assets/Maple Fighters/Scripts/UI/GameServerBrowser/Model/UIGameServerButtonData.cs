namespace Scripts.UI.GameServerBrowser
{
    public struct UIGameServerButtonData
    {
        public string Name { get; }

        public string Protocol { get; }

        public string Url { get; }

        public UIGameServerButtonData(string name, string protocol, string url)
        {
            Name = name;
            Protocol = protocol;
            Url = url;
        }
    }
}
