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

        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer(Player player)
        {
            //todo: add player to database
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = player.Id }, player);
        }

        [HttpGet("{id}")]
        private async Task<ActionResult<IEnumerable<Player>>> Get(string id)
        {
            return await _context.Players.ToListAsync();
        }
    }
}