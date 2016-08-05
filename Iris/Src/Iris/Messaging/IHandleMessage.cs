﻿namespace Iris.Messaging
{
    public interface IHandleMessage<in TMessage> where TMessage : class 
    {
        void Handle(TMessage m);
    }
}
