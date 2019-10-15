using CommonTools.Coroutines;
using UnityEngine;

namespace Network.Utils
{
    public class GameTimeProvider : ITimeProvider
    {
        public float GetCurrentTime()
        {
            return Time.time;
        }
    }
}