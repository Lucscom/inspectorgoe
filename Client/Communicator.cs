using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GameComponents.Model;
using GameComponents;
using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.SignalR.Client;

namespace Client
{

    public class Communicator
    {
        /// <summary>
        /// Http Client manages connection
        /// </summary>
        private HttpClient _client;
        /// <summary>
        /// Bearer Token for authorization on the server
        /// </summary>
        private String _token;
        /// <summary>
        /// Server URL
        /// </summary>
        private String _url;

        public GameState GameState { get; set; }

        /// <summary>
        /// Constructor for the communicator without url
        /// </summary>
        public Communicator()
        {
            _client = new HttpClient();

        }

        /// <summary>
        /// Initialize the Communicator with the Server URL
        /// </summary>
        /// <param name="url">Server URL</param>
        public void initClient(String url)

        {
            _url = url;
            _client.BaseAddress = new Uri(url);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

        }

        /// <summary>
        /// Set the Bearer Token for Authorization and configure the HTTP Header
        /// </summary>
        private void setToken()
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _token);
        }

        /// <summary>
        /// Create(Http Post) a new Player (Register)
        /// </summary>
        /// <param name="player">Player Object</param>
        /// <returns>Http Status Code</returns>
        public async Task<HttpStatusCode> CreatePlayerAsync(Player player)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync(
                "api/Player", player);
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        /// <summary>
        /// Login to an existing Player to get the authorization Token
        /// (Http Post)
        /// </summary>
        /// <param name="player">Player Object</param>
        /// <returns>Http Status Code</returns>
        public async Task<HttpStatusCode> LoginAsync(Player player)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync(
                "api/Player/login", player);
            response.EnsureSuccessStatusCode();
            String tokenJson = await response.Content.ReadAsStringAsync();
            _token = System.Text.Json.JsonSerializer.Deserialize<Token>(
                tokenJson).token;
            setToken();
            return response.StatusCode;
        }

        /// <summary>
        /// Make a move with a destination POI and a ticket type
        /// (Http Put)
        /// </summary>
        /// <param name="move">Move Player Object</param>
        /// <returns>Http Status Code</returns>
        public async Task<HttpStatusCode> MovePlayerAsync(MovePlayerDto move)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync(
                "api/Player", move);
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        /// <summary>
        /// Initializes hub connection to server with token as authentication
        /// </summary>
        /// <param name="token">Used for authentication</param>
        /// <returns></returns>
        public async Task RegisterGameHubAsync()
        {
            HubConnection connection;
            connection = new HubConnectionBuilder()
                .WithUrl(_url, options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(_token);
                })
                .WithAutomaticReconnect()
                .Build();

            //Register method that can be called from the server
            connection.On<GameState>("ReceiveGameState", (gameState) =>
            {
                GameState = gameState;
            });

            var t = connection.StartAsync();

            t.Wait();
        }
    }
}