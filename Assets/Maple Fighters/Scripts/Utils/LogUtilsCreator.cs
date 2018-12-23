using CommonTools.Log;
using UnityEngine;

namespace Scripts.Services
{
    public class LogUtilsCreator : MonoBehaviour
    {
        private void OnAwake()
        {
            LogUtils.Logger = new Logger();

            Destroy(gameObject);
        }
    }
}