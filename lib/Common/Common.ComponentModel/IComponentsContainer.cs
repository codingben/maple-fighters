﻿using System;
using System.Collections.Generic;

namespace Common.ComponentModel
{
    public interface IComponentsContainer : IDisposable
    {
        void TryAdd<TComponent>(TComponent component)
            where TComponent : class;

        void TryAddExposedOnly<TComponent>(TComponent component)
            where TComponent : class;

        TComponent Remove<TComponent>()
            where TComponent : class;

        TComponent Find<TComponent>()
            where TComponent : class;

        IEnumerable<object> GetAll();
    }
}