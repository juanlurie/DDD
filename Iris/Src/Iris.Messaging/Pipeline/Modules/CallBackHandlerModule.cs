using System;
using Iris.Logging;
using Iris.Messaging.Callbacks;
using Iris.Pipes;

namespace Iris.Messaging.Pipeline.Modules
{
    public class CallBackHandlerModule : IModule<IncomingMessageContext>
    {
        private readonly IManageCallbacks callBackManager;
        private readonly static ILog Logger = LogFactory.BuildLogger(typeof(CallBackHandlerModule));

        public CallBackHandlerModule(IManageCallbacks callBackManager)
        {
            this.callBackManager = callBackManager;
        }

        public bool Process(IncomingMessageContext input, Func<bool> next)
        {
            if (input.IsLocalMessage)
                return next();

            Logger.Debug("Attempting to dispatch message {0} to registered callbacks.", input);
            callBackManager.HandleCallback(input);
            return next();
        }
    }
}