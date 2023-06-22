using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace InspectorGoeServer.Hubs
{
    /// <summary>
    /// Used for Server Client communication
    /// The Hub enables server to push data to clients
    /// </summary>
    [Authorize]
    public class GameHub : Hub
    {
        //TODO: remove
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
