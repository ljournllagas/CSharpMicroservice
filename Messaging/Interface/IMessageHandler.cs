using System;
using System.Collections.Generic;
using System.Text;

namespace Messaging.Interface
{
    public interface IMessageHandler
    {
        void Start(IMessageHandlerCallback callback);
        void Stop();
    }
}
