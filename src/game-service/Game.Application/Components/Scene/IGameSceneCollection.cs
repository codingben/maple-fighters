﻿using System;
using System.Collections.Generic;

namespace Game.Application.Components
{
    public interface IGameSceneCollection : IDisposable
    {
        bool Add(string name, IGameScene gameScene);

        bool Remove(string name);

        bool TryGet(string name, out IGameScene gameScene);

        IEnumerable<IGameScene> GetAll();
    }
}