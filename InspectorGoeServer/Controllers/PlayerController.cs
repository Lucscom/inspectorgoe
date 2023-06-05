using GameComponents;
using GameComponents.Model;
using InspectorGoeServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

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
        private async Task<ActionResult<Player>> GetPlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            return Ok(player);
        }

        [HttpPost]
        [ActionName(nameof(PostPlayer))]
        public async Task<ActionResult<Player>> PostPlayer(Player player)
        {
            //todo: add player to database
            var newPlayer = _context.Players.Add(player);
            await _context.SaveChangesAsync();
            return Ok();
            //return CreatedAtAction(nameof(GetPlayer), new { id = newPlayer.Entity.Id }, newPlayer);
        }

        [HttpPut("{id}")]
        [ActionName(nameof(PutPlayer))]
        public async Task<IActionResult> PutPlayer(int id, PointOfInterest poi, TicketTypeEnum ticketType)
        {
            Player player = await _context.Players.FindAsync(id);
            GameComponents.Controller.GetInstance().MovePlayer(player, poi, ticketType);

            _context.Entry(player).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}