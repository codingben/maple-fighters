using CommonTools.Coroutines;
using UnityEngine;

namespace Scripts.Network
{
    public class GameTimeProvider : ITimeProvider
    {
        public float GetCurrentTime()
        {
            return Time.time;
        }
    }
}