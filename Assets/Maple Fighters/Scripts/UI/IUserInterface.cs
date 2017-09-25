using UnityEngine;

namespace Scripts.UI
{
    public interface IUserInterface
    {
        GameObject GameObject { get; }

        void Show();
        void Hide();
    }
}