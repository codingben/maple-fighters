using UnityEngine;

namespace Scripts.UI.Core
{
    public interface IUserInterface
    {
        GameObject GameObject { get; }

        void Show();
        void Hide();
    }
}