using CommonTools.Coroutines;
using UnityEngine;

namespace Scripts.Services
{
    public class GameTimeProvider : ITimeProvider
    {
        public float GetCurrentTime()
        {
            return Time.time;
        }
    }
}