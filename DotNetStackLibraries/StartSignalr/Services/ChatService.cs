using Microsoft.AspNet.SignalR;
using StartSignalr.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StartSignalr.Services
{
    public class ChatService
    {
        //单例
        private readonly static Lazy<ChatService> _instance = new Lazy<ChatService>(
            () => new ChatService(GlobalHost.ConnectionManager.GetHubContext<ChatHub>()));

        private readonly IHubContext _hubContext;

        public ChatService(IHubContext hubContext)
        {
            _hubContext = hubContext;
        }

        public void CallClientMethods()
        {
            _hubContext.Clients.All.method1();
        }
    }
}