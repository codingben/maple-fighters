using CommonTools.Coroutines;

namespace Scripts.Coroutines
{
    public static class ExtensionMethods
    {
        public static ExternalCoroutinesExecutor ExecuteExternally(
            this ExternalCoroutinesExecutor executer)
        {
            CoroutinesWrappersUpdater.GetOrCreateInstance()
                ?.AddCoroutineExecutor(executer);

            return executer;
        }

        public static ExternalCoroutinesExecutor RemoveFromExternalExecutor(
            this ExternalCoroutinesExecutor executer)
        {
            CoroutinesWrappersUpdater.GetOrCreateInstance()
                ?.RemoveCoroutineExecutor(executer);

            return executer;
        }
    }
}