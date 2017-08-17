namespace ServerApplication.Common.ComponentModel
{
    public static class ServerComponents
    {
        public static IComponentsContainer Container { get; } = new ComponentsContainer();
    }
}