using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xer.Delegator.Resolvers;

namespace Xer.Delegator.Registrations
{
    public class MultiMessageHandlerRegistration : IMessageHandlerRegistration
    {
        #region Declarations

        private readonly MultiMessageHandlerDelegateStore _messageHandlersByMessageType = new MultiMessageHandlerDelegateStore();

        #endregion Declarations

        #region IMessageHandlerRegistration Implementation

        /// <summary>
        /// Register an asynchronous message handler delegate for the specified message type. 
        /// This will add the message handler delegate to an internal collection of delegates.
        /// </summary>
        /// <typeparam name="TMessage">Type of message.</typeparam>
        /// <param name="messageHandlerDelegate">Asynchronous message handler delegate.</param>
        public void Register<TMessage>(MessageHandlerDelegate<TMessage> messageHandler) where TMessage : class
        {
            if (messageHandler == null)
            {
                throw new ArgumentNullException(nameof(messageHandler));
            }

            _messageHandlersByMessageType.Add<TMessage>(messageHandler);
        }

        /// <summary>
        /// Register a synchronous message handler delegate for the specified message type. 
        /// This will add the message handler delegate to an internal collection of delegates.
        /// </summary>
        /// <typeparam name="TMessage">Type of message.</typeparam>
        /// <param name="messageHandlerDelegate">Synchronous message handler delegate.</param>
        public void Register<TMessage>(Action<TMessage> messageHandler) where TMessage : class
        {
            if (messageHandler == null)
            {
                throw new ArgumentNullException(nameof(messageHandler));
            }
            
            // Convert to async delegate.
            Register<TMessage>((message, ct) =>
            {
                messageHandler.Invoke(message);
                return TaskUtility.CompletedTask;
            });
        }

        #endregion IMessageHandlerRegistration Implementation

        #region Methods
        
        /// <summary>
        /// Build a message handler resolver containing all registered message handler delegates.
        /// </summary>
        /// <returns>
        /// Message handler resolver that returns a message handler delegate 
        /// which invokes all stored delegates in the internal collection of delegates.
        /// </returns>
        public IMessageHandlerResolver BuildMessageHandlerResolver()
        {
            return new MultiMessageHandlerResolver(_messageHandlersByMessageType);
        }

        #endregion Methods
    }
}