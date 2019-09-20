using System;

namespace Scripts.UI.Minimap
{
    public interface IMinimapView
    {
        event Action<int> MarkSelectionChanged;
    }
}