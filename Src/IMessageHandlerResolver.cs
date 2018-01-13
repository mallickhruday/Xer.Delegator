using System;

namespace Xer.Delegator
{
    public interface IMessageHandlerResolver
    {
        /// <summary>
        /// Resolve a message handler delegate for the message.
        /// </summary>
        /// <typeparam name="TMessage">Type of message.</typeparam>
        /// <returns>Message handler delegate.</returns>
        MessageHandlerDelegate<TMessage> ResolveMessageHandler<TMessage>() where TMessage : class;
    }
}