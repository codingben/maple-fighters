using CommonTools.Coroutines;

namespace Scripts.Coroutines
{
    public static class ExtensionMethods
    {
        public static void ExecuteExternally(
            this ExternalCoroutinesExecutor executer)
        {
            CoroutinesWrappersUpdater.GetOrCreateInstance()
                .AddCoroutineExecutor(executer);
        }

        public static void RemoveFromExternalExecutor(
            this ExternalCoroutinesExecutor executer)
        {
            CoroutinesWrappersUpdater.GetOrCreateInstance()
                .RemoveCoroutineExecutor(executer);
        }
    }
}