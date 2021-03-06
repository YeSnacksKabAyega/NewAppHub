﻿using Microsoft.AspNetCore.SignalR;

namespace NewAppHub
{
    public class MessageHub : Hub
    {
        public void SendToAll(string name, string message)
        {
            Clients.All.InvokeAsync("sendToAll", name, message);
        }
    }
}