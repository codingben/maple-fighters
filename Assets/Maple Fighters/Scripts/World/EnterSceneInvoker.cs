using CommonTools.Coroutines;
using Scripts.Containers;
using Scripts.Coroutines;
using Scripts.Utils;
using UnityEngine.SceneManagement;

namespace Scripts.World
{
    public class EnterSceneInvoker : DontDestroyOnLoad<EnterSceneInvoker>
    {
        private readonly ExternalCoroutinesExecutor coroutinesExecutor = new ExternalCoroutinesExecutor();

        protected override void OnAwake()
        {
            base.OnAwake();

            SubscribeToSceneLoaded();
        }

        private void Update()
        {
            coroutinesExecutor.Update();
        }

        private void OnDestroy()
        {
            coroutinesExecutor.RemoveFromExternalExecutor();
            coroutinesExecutor.Dispose();

            UnsubscribeFromSceneLoaded();
        }

        private void SubscribeToSceneLoaded()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void UnsubscribeFromSceneLoaded()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            coroutinesExecutor.StartTask(ServiceContainer.GameService.EnterScene);
        }
    }
}