﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pipelines.Implementations.Pipelines;

namespace Pipelines.ExtensionMethods
{
    /// <summary>
    /// Extension methods for classes <see cref="IEnumerable{T}"/>
    /// where T is of type <see cref="IProcessor"/>.
    /// </summary>
    public static class EnumerableProcessorsExtensionMethods
    {
        /// <summary>
        /// Creates a pipeline from several processors.
        /// Allows to quickly create a pipeline and avoid
        /// declaring the class of <see cref="IPipeline"/>.
        /// </summary>
        /// <remarks>
        /// In case enumerable object is null, an empty
        /// pipeline will be returned.
        /// </remarks>
        /// <param name="enumerable">
        /// Collection of processors, to be used in pipeline.
        /// </param>
        /// <returns>
        /// Pipeline created from enumerable object of <see cref="IProcessor"/>.
        /// </returns>
        public static IPipeline ToPipeline(this IEnumerable<IProcessor> enumerable)
        {
            if (enumerable.IsNull())
            {
                return PredefinedPipeline.Empty;
            }

            return PredefinedPipeline.FromProcessors(enumerable);
        }

        /// <summary>
        /// Generic interpretation of <see cref="ToPipeline"/>,
        /// creates a pipeline from several processors.
        /// Allows to quickly create a pipeline and avoid
        /// declaring the class of <see cref="IPipeline"/>.
        /// This method allows to keep type of an object during
        /// extension method call.
        /// </summary>
        /// <remarks>
        /// In case enumerable object is null, an empty
        /// pipeline will be returned.
        /// </remarks>
        /// <typeparam name="TArgs">
        /// A type that is declared to be handled by processor.
        /// </typeparam>
        /// <param name="enumerable">
        /// Collection of processors, to be used in pipeline.
        /// </param>
        /// <returns>
        /// Pipeline created from enumerable object of <see cref="IProcessor"/>.
        /// </returns>
        public static SafeTypePipeline<TArgs> ToPipeline<TArgs>(this IEnumerable<SafeTypeProcessor<TArgs>> enumerable)
        {
            if (enumerable.IsNull())
            {
                return PredefinedPipeline.GetEmpty<TArgs>();
            }

            return PredefinedPipeline.FromProcessors(enumerable);
        }

        /// <summary>
        /// Repeats processors of the <paramref name="enumerable"/>,
        /// while the condition specified in parameter
        /// <paramref name="condition"/> returns <c>true</c>.
        /// </summary>
        /// <remarks>
        /// If <paramref name="enumerable"/> or <paramref name="condition"/>
        /// is null, returns no objects.
        /// </remarks>
        /// <param name="enumerable">
        /// Collection of processors, to be used in pipeline.
        /// </param>
        /// <param name="condition">
        /// Function, that returns a value indicating whether processors
        /// of the <paramref name="enumerable"/> have to be repeated
        /// and returned one more time.
        /// </param>
        /// <returns>
        /// Enumerable object retutning instances of <see cref="IProcessor"/>
        /// repeated as many times as <paramref name="condition"/> returned <c>true</c>.
        /// </returns>
        public static IEnumerable<IProcessor> RepeatProcessorsWhile(
            this IEnumerable<IProcessor> enumerable, Func<bool> condition)
        {
            if (condition.HasNoValue() || enumerable.HasNoValue())
            {
                yield break;
            }

            while (condition())
            {
                foreach (var processor in enumerable)
                {
                    yield return processor;
                }
            }
        }

        /// <summary>
        /// Generic version of <see cref="RepeatProcessorsWhile"/>,
        /// repeats processors of the <paramref name="enumerable"/>,
        /// while the condition specified in parameter
        /// <paramref name="condition"/> returns <c>true</c>.
        /// This method allows to keep type of an object during
        /// extension method call.
        /// </summary>
        /// <remarks>
        /// If <paramref name="enumerable"/> or <paramref name="condition"/>
        /// is null, returns no objects.
        /// </remarks>
        /// <typeparam name="TArgs">
        /// A type that is declared to be handled by processor.
        /// </typeparam>
        /// <param name="enumerable">
        /// Collection of processors, to be used in pipeline.
        /// </param>
        /// <param name="condition">
        /// Function, that returns a value indicating whether processors
        /// of the <paramref name="enumerable"/> have to be repeated
        /// and returned one more time.
        /// </param>
        /// <returns>
        /// Enumerable object retutning instances of <see cref="IProcessor"/>
        /// repeated as many times as <paramref name="condition"/> returned <c>true</c>.
        /// </returns>
        public static IEnumerable<SafeTypeProcessor<TArgs>> RepeatProcessorsWhile<TArgs>(
            this IEnumerable<SafeTypeProcessor<TArgs>> enumerable, Func<bool> condition)
        {
            if (condition.HasNoValue() || enumerable.HasNoValue())
            {
                yield break;
            }

            while (condition())
            {
                foreach (var processor in enumerable)
                {
                    yield return processor;
                }
            }
        }

        /// <summary>
        /// Creates a pipeline, which returns processors
        /// while the condition specified in parameter
        /// <paramref name="condition"/> returns <c>true</c>.
        /// </summary>
        /// <remarks>
        /// If <paramref name="enumerable"/> is null, returns an empty
        /// pipeline object.
        /// </remarks>
        /// <param name="enumerable">
        /// Collection of processors, to be used in pipeline.
        /// </param>
        /// <param name="condition">
        /// Function, that returns a value indicating whether processors
        /// of the <paramref name="enumerable"/> have to be repeated
        /// and returned one more time.
        /// </param>
        /// <returns>
        /// Pipeline object retutning instances of <see cref="IProcessor"/>
        /// repeated as many times as <paramref name="condition"/> returned <c>true</c>.
        /// </returns>
        public static IPipeline RepeatProcessorsAsPipelineWhile(
            this IEnumerable<IProcessor> enumerable, Func<bool> condition)
        {
            if (enumerable.HasNoValue())
            {
                return PredefinedPipeline.Empty;
            }

            return new RepeatingProcessorsWhileConditionPipeline(enumerable, condition);
        }


        /// <summary>
        /// Generic version of the <see cref="RepeatProcessorsAsPipelineWhile"/>,
        /// creates a pipeline, which returns processors
        /// while the condition specified in parameter
        /// <paramref name="condition"/> returns <c>true</c>.
        /// This method allows to keep type of an object during
        /// extension method call.
        /// </summary>
        /// <remarks>
        /// If <paramref name="enumerable"/> or <paramref name="condition"/> is null,
        /// returns an empty pipeline object.
        /// </remarks>
        /// <typeparam name="TArgs">
        /// A type that is declared to be handled by processor.
        /// </typeparam>
        /// <param name="enumerable">
        /// Collection of processors, to be used in pipeline.
        /// </param>
        /// <param name="condition">
        /// Function, that returns a value indicating whether processors
        /// of the <paramref name="enumerable"/> have to be repeated
        /// and returned one more time.
        /// </param>
        /// <returns>
        /// Pipeline object retutning instances of <see cref="IProcessor"/>
        /// repeated as many times as <paramref name="condition"/> returned <c>true</c>.
        /// </returns>
        public static SafeTypePipeline<TArgs> RepeatProcessorsAsPipelineWhile<TArgs>(
            this IEnumerable<SafeTypeProcessor<TArgs>> enumerable, Func<bool> condition)
        {
            if (enumerable.IsNull() || condition.IsNull())
            {
                return PredefinedPipeline.GetEmpty<TArgs>();
            }

            return new RepeatingProcessorsWhileConditionPipeline<TArgs>(enumerable, condition);
        }

        /// <summary>
        /// Runs processors of <paramref name="enumerable"/> one by one
        /// with a default <see cref="PipelineRunner"/> instance.
        /// </summary>
        /// <typeparam name="TArgs">
        /// Type of arguments to be passed in each processor.
        /// </typeparam>
        /// <param name="enumerable">
        /// Enumerable object of <see cref="IProcessor"/> to be
        /// executed one by one with a default <see cref="PipelineRunner"/>.
        /// </param>
        /// <param name="args">Arguments to be passed in each processor.</param>
        /// <returns>
        /// Returns <see cref="Task"/> object, which indicates a status
        /// of execution of processors.
        /// </returns>
        public static Task Run<TArgs>(this IEnumerable<IProcessor> enumerable, TArgs args)
        {
            return enumerable.Run(args, PipelineRunner.StaticInstance);
        }

        /// <summary>
        /// Runs processors of <paramref name="enumerable"/> one by one
        /// with a specified <see cref="PipelineRunner"/> instance.
        /// </summary>
        /// <typeparam name="TArgs">
        /// Type of arguments to be passed in each processor.
        /// </typeparam>
        /// <param name="enumerable">
        /// Enumerable object of <see cref="IProcessor"/> to be
        /// executed one by one with a specified <see cref="PipelineRunner"/>.
        /// </param>
        /// <param name="args">Arguments to be passed in each processor.</param>
        /// <param name="runner">
        /// Pipeline runner instance which will be used
        /// to execute collection of processors.
        /// </param>
        /// <returns>
        /// Returns <see cref="Task"/> object, which indicates a status
        /// of execution of processors.
        /// </returns>
        public static async Task Run<TArgs>(this IEnumerable<IProcessor> enumerable, TArgs args, IProcessorRunner runner)
        {
            runner = runner.Ensure(PipelineRunner.StaticInstance);
            if (enumerable.HasNoValue())
            {
                return;
            }

            foreach (var processor in enumerable)
            {
                await runner.RunProcessor(processor, args);
            }
        }

        /// <summary>
        /// Runs processors of <paramref name="enumerable"/> one by one
        /// with a default <see cref="PipelineRunner"/> instance
        /// and repeats run each time condition in method <paramref name="condition"/>
        /// returns true.
        /// </summary>
        /// <typeparam name="TArgs">
        /// Type of arguments to be passed in each processor.
        /// </typeparam>
        /// <param name="enumerable">
        /// Enumerable object of <see cref="IProcessor"/> to be
        /// executed one by one with a default <see cref="PipelineRunner"/>
        /// while <paramref name="condition"/> returns <c>true</c>.
        /// </param>
        /// <param name="args">Arguments to be passed in each processor.</param>
        /// <param name="condition">
        /// Function, that returns a value indicating whether processors
        /// of the <paramref name="enumerable"/> have to be executed
        /// one more time.
        /// </param>
        /// <returns>
        /// Returns <see cref="Task"/> object, which indicates a status
        /// of execution of processors.
        /// </returns>
        public static Task RunProcessorsWhile<TArgs>(this IEnumerable<IProcessor> enumerable, TArgs args,
            Predicate<TArgs> condition)
        {
            return enumerable.RunProcessorsWhile(args, condition, PipelineRunner.StaticInstance);
        }

        /// <summary>
        /// Runs processors of <paramref name="enumerable"/> one by one
        /// with a specified <see cref="PipelineRunner"/> instance
        /// and repeats run each time condition in method <paramref name="condition"/>
        /// returns true.
        /// </summary>
        /// <typeparam name="TArgs">
        /// Type of arguments to be passed in each processor.
        /// </typeparam>
        /// <param name="enumerable">
        /// Enumerable object of <see cref="IProcessor"/> to be
        /// executed one by one with a specified <see cref="PipelineRunner"/>
        /// while <paramref name="condition"/> returns <c>true</c>.
        /// </param>
        /// <param name="args">Arguments to be passed in each processor.</param>
        /// <param name="condition">
        /// Function, that returns a value indicating whether processors
        /// of the <paramref name="enumerable"/> have to be executed
        /// one more time.
        /// </param>
        /// <param name="runner">
        /// Pipeline runner instance which will be used
        /// to execute collection of processors.
        /// </param>
        /// <returns>
        /// Returns <see cref="Task"/> object, which indicates a status
        /// of execution of processors.
        /// </returns>
        public static async Task RunProcessorsWhile<TArgs>(this IEnumerable<IProcessor> enumerable, TArgs args,
            Predicate<TArgs> condition, IProcessorRunner runner)
        {
            if (condition.HasNoValue())
            {
                return;
            }

            runner = runner.Ensure(PipelineRunner.StaticInstance);

            while (condition(args))
            {
                await enumerable.Run(args, runner);
            }
        }

        /// <summary>
        /// Method that allows add a processor into <paramref name="enumerable"/> object.
        /// </summary>
        /// <param name="enumerable">
        /// Enumerable object of <see cref="IProcessor"/> where <paramref name="nextProcessor"/>
        /// has to be added.
        /// </param>
        /// <param name="nextProcessor">
        /// A processor to be added into <paramref name="enumerable"/>.
        /// </param>
        /// <returns>
        /// Enumerable object with a concatenated processor.
        /// </returns>
        public static IEnumerable<IProcessor> ThenProcessor(this IEnumerable<IProcessor> enumerable, IProcessor nextProcessor)
        {
            return enumerable.ThenProcessors(new[] {nextProcessor});
        }

        /// <summary>
        /// Method that allows concatenate processors into <paramref name="enumerable"/> object.
        /// </summary>
        /// <param name="enumerable">
        /// Enumerable object of <see cref="IProcessor"/> where <paramref name="nextProcessors"/>
        /// has to be added.
        /// </param>
        /// <param name="nextProcessors">
        /// Processors to be added into <paramref name="enumerable"/>.
        /// </param>
        /// <returns>
        /// Enumerable object with concatenated processors.
        /// </returns>
        public static IEnumerable<IProcessor> ThenProcessors(this IEnumerable<IProcessor> enumerable, IEnumerable<IProcessor> nextProcessors)
        {
            if (enumerable.HasNoValue())
            {
                if (nextProcessors.HasNoValue())
                {
                    return Enumerable.Empty<IProcessor>();
                }

                return nextProcessors;
            }

            if (nextProcessors.HasNoValue())
            {
                return enumerable;
            }

            return enumerable.Concat(nextProcessors);
        }
    }
}