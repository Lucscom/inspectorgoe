using GameComponents;
using GameComponents.Model;
using InspectorGoeServer.Hubs;
using InspectorGoeServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        private readonly GameController _gameController;

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
            IHubContext<GameHub> hubContext,
            GameController gameController)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
            _hubContext = hubContext;
            _gameController = gameController;
        }

        /// <summary>
        /// Gets the authenticated player
        /// </summary>
        /// <returns>The authenticated Player if found</returns>
        [HttpGet]
        [Authorize]
        [ActionName(nameof(GetPlayer))]
        public async Task<ActionResult<Player>> GetPlayer()
        {
            var currentUser = (await _context.Players.ToListAsync()).Where(p => p.UserName == User.Identity.Name).First();
            return Ok(currentUser);
        }

        /// <summary>
        /// Registers a new player and adds it to the game
        /// </summary>
        /// <param name="player">Player</param>
        /// <returns>Ok if successfull</returns>
        [HttpPost("register")]
        [AllowAnonymous]
        [ActionName(nameof(RegisterPlayer))]
        public async Task<ActionResult<Player>> RegisterPlayer([FromBody] Player player)
        {
            var userResult = await _userManager.CreateAsync(player, player.Password);

            if (!userResult.Succeeded)
                return new BadRequestObjectResult(userResult);

            var newPlayer = await _context.Players.FindAsync(player.Id);

            if (newPlayer != null)
                return Created("", newPlayer);

            return StatusCode(500); //500 - Internal Server Error
        }

        /// <summary>
        /// Returns a token for the given user credentials
        /// </summary>
        /// <param name="user">Player object with username and password</param>
        /// <returns>Valid JWT bearer token</returns>
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
        /// Moves the player to the given point of interest
        /// </summary>
        /// <param name="movement">The movement parameters</param>
        /// <returns>Ok, Http response with no content</returns>
        [HttpPut("move")]
        [Authorize]
        [ActionName(nameof(MovePlayer))]
        public async Task<IActionResult> MovePlayer([FromBody] MovePlayerDto movement)
        {
            var currentUser = (await _context.Players.ToListAsync()).Where(p => p.UserName == User.Identity.Name).First(); //todo: clean this up
            if (currentUser == null)
                return StatusCode(500);

            try
            {
                if (_gameController.MovePlayer(currentUser, movement.PointOfInterest, movement.TicketType))
                {
                    await updateGameComponents(_gameController.GameState);
                    return Ok();
                }
            } catch (Exception ex)
            {
                if(ex.Message.Equals("MisterX found!"))
                {
                    await _hubContext.Clients.All.SendAsync("GameEnd", "MisterX");
                }
            }

            return BadRequest();
        }

        /// <summary>
        /// Starts the game
        /// </summary>
        /// <returns>Ok</returns>
        [HttpPut("creategame")]
        [Authorize]
        [ActionName(nameof(CreateGame))]
        public async Task<IActionResult> CreateGame()
        {
            var currentUser = (await _context.Players.ToListAsync()).Where(p => p.UserName == User.Identity.Name).First(); //todo: clean this up
            if (currentUser == null)
                return StatusCode(500);

            if (!_gameController.CreateGame(currentUser))
            {
                return BadRequest();
            }
            await updateGameComponents(_gameController.GameState);

            return Ok();
        }

        /// <summary>
        /// Starts the game
        /// </summary>
        /// <returns>Ok</returns>
        [HttpPut("startgame")]
        [Authorize]
        [ActionName(nameof(StartGame))]
        public async Task<IActionResult> StartGame()
        {
            if (!_gameController.StartGame())
            {
                return BadRequest();
            }
            await updateGameComponents(_gameController.GameState);

            return Ok();
        }

        /// <summary>
        /// Join the game
        /// </summary>
        /// <returns>Ok</returns>
        [HttpPut("joingame")]
        [Authorize]
        [ActionName(nameof(JoinGame))]
        public async Task<IActionResult> JoinGame()
        {
            var currentUser = (await _context.Players.ToListAsync()).Where(p => p.UserName == User.Identity.Name).First(); //todo: clean this up
            if (currentUser == null)
                return StatusCode(500);

            if (!_gameController.JoinGame(currentUser))
            {
                return BadRequest();
            }
            await updateGameComponents(_gameController.GameState);

            return Ok();
        }

        /// <summary>
        /// Send gameState to all clients
        /// </summary>
        /// <param name="gameState">The current gameState</param>
        private async Task updateGameComponents(GameState gameState)
        {
            await _hubContext.Clients.All.SendAsync("UpdateGameState", System.Text.Json.JsonSerializer.Serialize(gameState));
        }


        #region TokenGeneration
        /// <summary>
        /// Generates a JWT token for the given user
        /// </summary>
        /// <param name="user">player</param>
        /// <returns>JWT Token</returns>
        private async Task<string> GenerateToken(Player? user)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
        /// <summary>
        /// The key used to sign the JWT token
        /// </summary>
        /// <returns>The signing credentials</returns>
        private SigningCredentials GetSigningCredentials()
        {
            var jwtConfig = _configuration.GetSection("jwtConfig");
            var key = Encoding.UTF8.GetBytes(jwtConfig["Secret"]); //reads the secret from the appsettings.json
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        /// <summary>
        /// The claims the user has
        /// </summary>
        /// <param name="user">Player</param>
        /// <returns>List of claims</returns>
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
        /// Generates a JWT token representation
        /// </summary>
        /// <param name="signingCredentials">Credentials for signing</param>
        /// <param name="claims">List of claims</param>
        /// <returns>JWT token representation</returns>
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
        #endregion
    }
}