using System;

namespace Scripts.UI.Windows
{
    public interface IMinimapView
    {
        event Action<int> MarkSelectionChanged;
    }
}