using Microsoft.AspNetCore.SignalR;

namespace Hubs
{
    public class ChatHub : Hub
    {
        public string GetConnectionId() => Context.ConnectionId;
    }
}