using CommonTools.Coroutines;
using Scripts.Utils;

namespace Scripts.Services
{
    public class GameTimeProviderCreator : DontDestroyOnLoad<GameTimeProviderCreator>
    {
        protected override void OnAwake()
        {
            base.OnAwake();

            TimeProviders.DefaultTimeProvider = new GameTimeProvider();

            Destroy(gameObject);
        }
    }
}