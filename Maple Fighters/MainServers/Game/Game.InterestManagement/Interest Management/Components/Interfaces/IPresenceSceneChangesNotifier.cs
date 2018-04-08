using System;

namespace InterestManagement.Components.Interfaces
{
    public interface IPresenceSceneChangesNotifier
    {
        event Action<IScene> SceneChanged;
    }
}