using System;

namespace Scripts.UI.CharacterSelection
{
    public interface ILoadingAnimation
    {
        event Action Finished;
    }
}