using CommunicationModel;
using GameComponents.Model;
using InspectorGoeServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace InspectorGoeServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {

        private readonly PlayerContext _context;

        public PlayerController(PlayerContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            return await _context.Players.ToListAsync();
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetPlayer))]
        private async Task<ActionResult<Player>> GetPlayer(string id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            return player;
        }
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer(Player player)
        {
            //todo: add player to database
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPlayer), new { id = player.Name }, player);
        }
    }
}