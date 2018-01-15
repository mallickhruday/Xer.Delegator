using System;
using System.Collections.Generic;
using Xer.Delegator.Exceptions;

namespace Xer.Delegator.Resolvers
{
    public class CompositeMessageHandlerResolver : IMessageHandlerResolver
    {
        #region Declarations
            
        private readonly IEnumerable<IMessageHandlerResolver> _resolvers;

        #endregion Declarations

        #region Constructors
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="messageHandlerResolvers">List of message handler resolvers.</param>
        public CompositeMessageHandlerResolver(IEnumerable<IMessageHandlerResolver> messageHandlerResolvers)
        {
            _resolvers = messageHandlerResolvers ?? throw new ArgumentNullException(nameof(messageHandlerResolvers));
        }

        #endregion Constructors

        #region IMessageHandlerResolver Implementation
        
        /// <summary>
        /// Resolve a message handler delegate for the message type from multiple sources.
        /// This will try resolving a message handler delegate from all sources until a handler
        /// who is either not null or not equal to <see cref="Xer.Delegator.NullMessageHandlerDelegate{TMessage}.Value"/> is found.
        /// </summary>
        /// <remarks>
        /// If no handler is found, a <see cref="Xer.Delegator.NullMessageHandlerDelegate{TMessage}.Value"/> will be returned.
        /// Any exceptions thrown by the other sources will be propagated.
        /// </remarks>
        /// <typeparam name="TMessage">Type of message.</typeparam>
        /// <returns>Message handler delegate.</returns>
        public MessageHandlerDelegate<TMessage> ResolveMessageHandler<TMessage>() where TMessage : class
        {
            try
            {
                // Resolvers can either be a list of SingleMessageHandlerResolver or MultiMessageHandlerResolver.
                foreach (IMessageHandlerResolver resolver in _resolvers)
                {
                    MessageHandlerDelegate<TMessage> messageHandlerDelegate = resolver.ResolveMessageHandler<TMessage>();
                    
                    if (messageHandlerDelegate != null && 
                        messageHandlerDelegate != NullMessageHandlerDelegate<TMessage>.Value)
                    {
                        return messageHandlerDelegate;
                    }
                }

                // Return null handler that does nothing.
                return NullMessageHandlerDelegate<TMessage>.Value;
            }
            catch(NoMessageHandlerResolvedException)
            {
                // If a source has thrown this exception, just rethrow.
                throw;
            }
            catch(Exception ex)
            {
                throw NoMessageHandlerResolvedException.FromMessageType(typeof(TMessage), ex);
            }
        }

        #endregion IMessageHandlerResolver Implementation
    }
}