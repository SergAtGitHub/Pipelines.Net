﻿using System;
using FluentAssertions;
using Moq;
using Pipelines.Implementations.Pipelines;
using Pipelines.Implementations.Runners;
using Xunit;

namespace Pipelines.Tests.Units
{
    public class ObservablePipelineRunnerTests
    {
        [Fact]
        public async void RunPipeline_Executes_Actions_On_Running_Is_Completed()
        {
            bool completed = false;
            
            // The runner to be tested.
            ObservablePipelineRunner runner = new ObservablePipelineRunner();

            // Creating an implementation of observer.
            Mock<IObserver<RunningPipelineObservableInformation>> mockObserver =
                new Mock<IObserver<RunningPipelineObservableInformation>>();
            mockObserver
                .Setup(observer => observer.OnCompleted())
                .Callback(() => completed = true);

            // Creating an implementation of pipeline.
            Mock<IPipeline> mockPipeline = new Mock<IPipeline>();


            using (runner.Subscribe(mockObserver.Object))
            {
                await runner.RunPipeline<object>(mockPipeline.Object, null);
            }


            completed.Should().BeTrue("because method on complete must trigger the flag");
        }

        [Fact]
        public async void Subscribe_Returns_A_Disposable_Object_Which_Removes_The_Subsriber_From_The_Collection()
        {
            bool completed = false;

            // The runner to be tested.
            ObservablePipelineRunner runner = new ObservablePipelineRunner();

            // Creating an implementation of observer.
            Mock<IObserver<RunningPipelineObservableInformation>> mockObserver =
                new Mock<IObserver<RunningPipelineObservableInformation>>();
            mockObserver
                .Setup(observer => observer.OnCompleted())
                .Callback(() => completed = true);

            // Creating an implementation of pipeline.
            Mock<IPipeline> mockPipeline = new Mock<IPipeline>();
            

            runner.Subscribe(mockObserver.Object).Dispose();
            await runner.RunPipeline<object>(mockPipeline.Object, null);


            completed.Should().BeFalse("because the subscriber was disposed before the RunPipeline was called");
        }

        [Fact]
        public async void OnError_Should_Be_Called_When_Error_Is_Not_Handled_In_Pipeline()
        {
            Exception exception = new NotImplementedException("Test exception");

            // The runner to be tested.
            ObservablePipelineRunner runner = new ObservablePipelineRunner();

            // Creating an implementation of observer.
            Mock<IObserver<RunningPipelineObservableInformation>> mockObserver =
                new Mock<IObserver<RunningPipelineObservableInformation>>();
            mockObserver
                .Setup(observer => observer.OnError(It.Is<Exception>(er => er == exception)))
                .Verifiable("The exception thrown must be the same as specified in test method");

            // Creating a processor that throws an exception.
            Mock<IProcessor> mockProcessor = new Mock<IProcessor>();
            mockProcessor.Setup(x => x.Execute(It.IsAny<object>())).Callback(() => throw exception);

            // Creating an implementation of pipeline.
            IPipeline mockPipeline = PredefinedPipeline.FromProcessors(mockProcessor.Object);


            using (runner.Subscribe(mockObserver.Object))
            {
                await runner.RunPipeline<object>(mockPipeline, null);
            }


            mockObserver.Verify();
        }
    }
}