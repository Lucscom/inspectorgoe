using GameComponents;
using GameComponents.Model;
using InspectorGoeServer.Hubs;
using InspectorGoeServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.Design;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InspectorGoeServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<PlayerController> _logger;
        /// <summary>
        /// Player Database
        /// </summary>
        private readonly PlayerContext _context;
        /// <summary>
        /// User Database
        /// </summary>
        private readonly UserManager<Player> _userManager;
        /// <summary>
        /// 
        /// </summary>
        private readonly IConfiguration _configuration;
        /// <summary>
        /// 
        /// </summary>
        private readonly IHubContext<GameHub> _hubContext;

        /// <summary>
        /// Constructor to init the player controller
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="configuration"></param>
        /// <param name="hubContext"></param>
        public PlayerController(
            ILogger<PlayerController> logger, 
            PlayerContext context,
            UserManager<Player> userManager,
            IConfiguration configuration, 
            IHubContext<GameHub> hubContext)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Http Post implementation on api/Player to register a new player
        /// </summary>
        /// <param name="player">Player object to be registered</param>
        /// <returns>Http Status code </returns>
        [HttpPost]
        [AllowAnonymous]
        [ActionName(nameof(RegisterPlayer))]
        public async Task<ActionResult<Player>> RegisterPlayer([FromBody] Player player)
        {
            var userResult = await _userManager.CreateAsync(player, player.Password);
            return !userResult.Succeeded ? 
                new BadRequestObjectResult(userResult) : StatusCode(201);
        }

        /// <summary>
        /// Http Post implementation on api/Player/login to login a Player
        /// and link the player with a bearer token for authentication
        /// </summary>
        /// <param name="user"> Player Object</param>
        /// <returns>Token Object with token string</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] Player user)
        {
            var currentUser = await _userManager.FindByNameAsync(user.UserName);
            var result = currentUser != null && await _userManager.CheckPasswordAsync(currentUser, user.Password);
            return !result
                ? Unauthorized()
                : Ok(new { Token = await GenerateToken(currentUser) });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<string> GenerateToken(Player? user)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private SigningCredentials GetSigningCredentials()
        {
            var jwtConfig = _configuration.GetSection("jwtConfig");
            var key = Encoding.UTF8.GetBytes(jwtConfig["Secret"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<List<Claim>> GetClaims(Player? user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signingCredentials"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtConfig = _configuration.GetSection("jwtConfig");
            var tokenOptions = new JwtSecurityToken(
                            issuer: jwtConfig["validIssuer"],
                            audience: jwtConfig["validAudience"],
                            claims: claims,
                            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtConfig["expiresIn"])),
                            signingCredentials: signingCredentials);
            return tokenOptions;
        }

        /// <summary>
        /// Http put implementation on api/Player to receive the move made by
        /// a user
        /// </summary>
        /// <param name="movement">Movement Object with POI and Ticket</param>
        /// <returns>Http response with no content</returns>
        [HttpPut]
        [Authorize]
        [ActionName(nameof(PutPlayer))]
        public async Task<IActionResult> PutPlayer([FromBody] MovePlayerDto movement)
        {
            var currentUser = await _context.Players.FindAsync(User.Identity.Name);
            if(GameComponents.Controller.GetInstance().MovePlayer(currentUser, movement.PointOfInterest, movement.TicketType))
            {
                sendGameComponents(GameComponents.Controller.GetInstance().GameState);
            }

            //_context.Entry(currentUser).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameState"></param>
        private async void sendGameComponents(GameState gameState)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveGameState", gameState);
        }

        //TODO: remove
        [HttpGet("send")]
        [AllowAnonymous]
        [ActionName(nameof(SendGameState))]
        public async Task<IActionResult> SendGameState()
        {
            GameState gameState = new GameState();
            await _hubContext.Clients.All.SendAsync("ReceiveGameState", gameState);
            return Ok();
        }
    }
}