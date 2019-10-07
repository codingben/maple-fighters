using CommonTools.Coroutines;

namespace Scripts.Coroutines
{
    public static class ExtensionMethods
    {
        public static CoroutineWrapperUpdater CoroutineWrapperUpdater =>
            CoroutineWrapperUpdater.GetOrCreateInstance();

        public static void ExecuteExternally(
            this ExternalCoroutinesExecutor executer)
        {
            CoroutineWrapperUpdater?.AddCoroutineExecutor(executer);
        }

        public static void RemoveFromExternalExecutor(
            this ExternalCoroutinesExecutor executer)
        {
            CoroutineWrapperUpdater?.RemoveCoroutineExecutor(executer);
        }
    }
}