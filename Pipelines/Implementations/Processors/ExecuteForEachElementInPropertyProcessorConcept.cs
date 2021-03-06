﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Pipelines.ExtensionMethods;

namespace Pipelines.Implementations.Processors
{
    public abstract class ExecuteForEachElementInPropertyProcessorConcept<TContext, TElement> 
        : ExecuteActionForPropertyProcessorConcept<TContext, IEnumerable<TElement>> where TContext : PipelineContext
    {
        public override Task PropertyExecution(TContext args, IEnumerable<TElement> property)
        {
            return this.CollectionExecution(args, property);
        }

        public virtual async Task CollectionExecution(TContext args, IEnumerable<TElement> collection)
        {
            foreach (var element in collection)
            {
                await ElementExecution(args, element);
            }
        }

        public abstract Task ElementExecution(TContext args, TElement element);
    }
}