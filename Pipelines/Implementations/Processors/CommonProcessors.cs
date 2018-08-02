﻿using System;
using System.Threading.Tasks;

namespace Pipelines.Implementations.Processors
{
    public static class CommonProcessors
    {
        public static ActionProcessor<TArgs> Action<TArgs>(Action<TArgs> action)
        {
            return ActionProcessor.FromAction<TArgs>(action);
        }

        public static ActionProcessor<TArgs> Action<TArgs>(Func<TArgs, Task> action)
        {
            return ActionProcessor.FromAction<TArgs>(action);
        }

        public static EnsurePropertyProcessor<TValue> EnsureProperty<TValue>(string name, TValue value)
        {
            return new EnsurePropertyProcessor<TValue>(name, value);
        }

        public static ExecuteForEachElementInPropertyProcessor<TElement> ExecuteForEachElementInProperty<TElement>(
            Action<TElement> action, string propertyName)
        {
            return new ExecuteForEachElementInPropertyProcessor<TElement>(action, propertyName);
        }
    }
}