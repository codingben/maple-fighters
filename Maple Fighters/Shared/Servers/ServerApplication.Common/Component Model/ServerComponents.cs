namespace ServerApplication.Common.ComponentModel
{
    public static class ServerComponents
    {
        public static IContainer Container { get; } = new Container<CommonComponent>();
    }
}