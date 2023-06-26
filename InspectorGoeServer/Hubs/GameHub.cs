using GameComponents.Model;
using InspectorGoeServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace InspectorGoeServer.Hubs
{
    /// <summary>
    /// Used for Server Client communication
    /// The Hub enables server to push data to clients
    /// </summary>
    [Authorize]
    public class GameHub : Hub
    {

        /// <summary>
        /// Player Database
        /// </summary>
        private readonly PlayerContext _context;
        /// <summary>
        /// User Database
        /// </summary>
        private readonly UserManager<Player> _userManager;

        public GameHub(PlayerContext playerContext, UserManager<Player> userManager) 
        {
            _context = playerContext;
            _userManager = userManager;
        }

        public override async Task OnConnectedAsync()
        {
            if (_userManager.Users.Count() < 1)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "MisterX");
            }
            else
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Detectives");
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
