using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using System.Web.Http;

namespace AppHub
{

    public static class UserHandler //this static class is to store the number of users conected at the same time
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
    }

    [HubName("Messenger")]
    public class MessageHub : Hub
    {
        public void sendMessage(string senderID, string message)
        {
            Clients.Others.showMessage(senderID, message);
        }

        public override Task OnConnected() //override OnConnect, OnReconnected and OnDisconnected to know if a user is connected or disconnected
        {
            UserHandler.ConnectedIds.Add(Context.ConnectionId); //add a connection id to the list
            Clients.All.usersConnected(UserHandler.ConnectedIds.Count()); //this will send to ALL the clients the number of users connected
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            Clients.All.usersConnected(UserHandler.ConnectedIds.Count());
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool v = false)
        {
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            Clients.All.usersConnected(UserHandler.ConnectedIds.Count());
            return base.OnDisconnected(v);
        }
    }
}