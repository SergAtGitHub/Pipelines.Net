﻿using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Pipelines.Tests.Units
{
    public class SafeTypeProcessorTests
    {
        [Fact]
        public async Task Safe_Execution_Is_Not_Reached_When_There_Is_An_Incorrect_Type()
        {
            var processor = new Mock<SafeTypeProcessor<string>>();
            await processor.Object.Execute(false);
            processor.Verify(p => p.SafeExecute(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Safe_Condition_Is_Reached_When_Type_Is_Correct()
        {
            var processor = new Mock<SafeTypeProcessor<string>>();
            await processor.Object.Execute(string.Empty);
            processor.Verify(p => p.SafeCondition(It.IsAny<string>()), Times.AtLeastOnce);
        }
        
        [Fact]
        public async Task Safe_Execution_Is_Not_Reached_When_Safe_Condition_Returns_False()
        {
            var processor = new Mock<SafeTypeProcessor<string>>();
            processor.Setup(x => x.SafeCondition(It.IsAny<string>())).Returns(false);
            await processor.Object.Execute(string.Empty);
            processor.Verify(p => p.SafeExecute(It.IsAny<string>()), Times.Never);
        }


        [Fact]
        public async Task Safe_Execution_Is_Reached_When_Safe_Condition_Returns_False()
        {
            var processor = new Mock<SafeTypeProcessor<string>>();
            processor.Setup(x => x.SafeCondition(It.IsAny<string>())).Returns(true);
            await processor.Object.Execute(string.Empty);
            processor.Verify(p => p.SafeExecute(It.IsAny<string>()), Times.AtLeastOnce);
        }
        
        [Fact]
        public async Task Safe_Processor_Makes_A_Check_Of_Safe_Condition_Before_Doing_Safe_Execution()
        {
            var processor = new Mock<SafeTypeProcessor<string>>(MockBehavior.Strict);

            var executionSequence = new MockSequence();
            processor.InSequence(executionSequence).Setup(x => x.SafeCondition(It.IsAny<string>())).Returns(true);
            processor.InSequence(executionSequence).Setup(x => x.SafeExecute(It.IsAny<string>())).Returns(PipelineTask.CompletedTask);

            await processor.Object.Execute(string.Empty);
        }
    }
}