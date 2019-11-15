﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Interface
{
    public interface IMessageHandlerCallback
    {
        Task<bool> HandleMessageAsync(string messageType, string message);
    }
}
