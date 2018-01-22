using System.Threading.Tasks;
using FluentAssertions;
using Xer.Delegator.Tests.Entities;
using Xunit;

namespace Xer.Delegator.Tests
{
    public class NullMessageHandlerDelegateTests
    {
        #region Value Field

        public class ValueField
        {
            [Fact]
            public void ShouldBeEqualIfMessageTypeIsTheSame()
            {
                MessageHandlerDelegate<TestMessage> handler1 = NullMessageHandlerDelegate<TestMessage>.Value;
                MessageHandlerDelegate<TestMessage> handler2 = NullMessageHandlerDelegate<TestMessage>.Value;

                handler1.Should().BeSameAs(handler2);
            }

            [Fact]
            public void ShouldNotBeEqualIfMessageTypeIsDifferent()
            {
                MessageHandlerDelegate<TestMessage> handler1 = NullMessageHandlerDelegate<TestMessage>.Value;
                MessageHandlerDelegate<string> handler2 = NullMessageHandlerDelegate<string>.Value;

                handler1.Should().NotBeSameAs(handler2);
            }

            [Fact]
            public void ShouldNotBeEqualToANewEmptyInstance()
            {
                MessageHandlerDelegate<TestMessage> handler1 = NullMessageHandlerDelegate<TestMessage>.Value;
                MessageHandlerDelegate<TestMessage> handler2 = (m, ct) => Task.CompletedTask;
                // Handler2 and Handler3 are equal. Why???
                // MessageHandlerDelegate<TestMessage> handler3 = (m, ct) => Task.CompletedTask;

                handler1.Should().NotBeSameAs(handler2);
            }
        }

        #endregion Value Field
    }
}