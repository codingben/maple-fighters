﻿using System;

namespace ComponentModel.Common
{
    public interface IComponent : IDisposable
    {
        void Awake(IContainer components);
    }

    public interface IComponent<in TOwner> : IDisposable
        where TOwner : IEntity<TOwner>
    {
        void Awake(TOwner entity);
    }
}