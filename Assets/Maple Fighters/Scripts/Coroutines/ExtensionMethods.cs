using CommonTools.Coroutines;

namespace Scripts.Coroutines
{
    public static class ExtensionMethods
    {
        public static void ExecuteExternally(
            this ExternalCoroutinesExecutor executer)
        {
            var coroutines = CoroutinesWrappersUpdater.GetOrCreateInstance();
            if (coroutines != null)
            {
                coroutines.AddCoroutineExecutor(executer);
            }
        }

        public static void RemoveFromExternalExecutor(
            this ExternalCoroutinesExecutor executer)
        {
            var coroutines = CoroutinesWrappersUpdater.GetOrCreateInstance();
            if (coroutines != null)
            {
                coroutines.RemoveCoroutineExecutor(executer);
            }
        }
    }
}