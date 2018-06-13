﻿using CommonTools.Log;
using ComponentModel.Common;
using InterestManagement.Components.Interfaces;

namespace InterestManagement.Components
{
    public class SceneObject : ISceneObject
    {
        public IContainer<ISceneObject> Components { get; }

        public int Id { get; }
        public string Name { get; }
        
        public SceneObject(int id, string name, TransformDetails transformDetails)
        {
            Id = id;
            Name = name;

            Components = new Container<ISceneObject>(this);
            Components.AddComponent(new Transform(transformDetails.Position, transformDetails.Size, transformDetails.Direction));
            Components.AddComponent(new PresenceSceneProvider());
        }

        public void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            // Left blank intentionally
        }

        protected virtual void OnDestroy()
        {
            // Left blank intentionally
        }

        public void Dispose()
        {
            OnDestroy();

            var presenceSceneProvider = Components.GetComponent<IPresenceSceneProvider>().AssertNotNull();
            presenceSceneProvider.GetScene()?.RemoveSceneObject(this);

            Components?.Dispose();
        }
    }
}