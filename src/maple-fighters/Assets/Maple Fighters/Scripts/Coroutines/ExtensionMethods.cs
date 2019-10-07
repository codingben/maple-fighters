using CommonTools.Coroutines;

namespace Scripts.Coroutines
{
    public static class ExtensionMethods
    {
        public static CoroutinesWrappersUpdater CoroutinesWrappersUpdater =>
            CoroutinesWrappersUpdater.GetOrCreateInstance();

        public static void ExecuteExternally(
            this ExternalCoroutinesExecutor executer)
        {
            CoroutinesWrappersUpdater?.AddCoroutineExecutor(executer);
        }

        public static void RemoveFromExternalExecutor(
            this ExternalCoroutinesExecutor executer)
        {
            CoroutinesWrappersUpdater?.RemoveCoroutineExecutor(executer);
        }
    }
}