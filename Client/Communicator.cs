﻿using System;
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
using Newtonsoft.Json;
using Client.Events;

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


        public AsyncAutoResetEvent gameStartedEvent = new AsyncAutoResetEvent(false);
        public AsyncAutoResetEvent newGameStateEvent = new AsyncAutoResetEvent(false);

        public event EventHandler UpdateGameStateEvent;
        public event EventHandler GameStartedEvent;

        public event EventHandler<GameEndEventArgs> GameEndEvent;

        /// <summary>
        /// Constructor for the communicator without url
        /// </summary>
        public Communicator()
        {
        }

        /// <summary>
        /// Initialize the Communicator with the Server URL
        /// </summary>
        /// <param name="url">Server URL</param>
        public void initClient(String url)

        {
            _url = url;
            _client = new HttpClient();
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
        /// Get own player Object from the Server
        /// </summary>
        /// <returns>Player player</returns>
        public async Task<Player> GetPlayerAsync()
        {
            HttpResponseMessage responseMessage = await _client.GetAsync("api/Player");
            responseMessage.EnsureSuccessStatusCode();
            String playerString = await responseMessage.Content.ReadAsStringAsync();
            Player player = JsonConvert.DeserializeObject<Player>(playerString);
            return player;
        }

        /// <summary>
        /// Create(Http Post) a new Player (Register)
        /// </summary>
        /// <param name="player">Player Object</param>
        /// <returns>Http Status Code</returns>
        public async Task<HttpResponseMessage> CreatePlayerAsync(Player player)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync(
                "api/Player/register", player);
            return response;
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
            var tokenObj = JsonConvert.DeserializeObject<StringDto>(tokenJson);
            _token = tokenObj.token;
            setToken();
            return response.StatusCode;
        }

        /// <summary>
        /// Pushes the serverside gamestate to all clients
        /// </summary>
        /// <returns>Http Status Code</returns>
        public async Task<HttpStatusCode> CreateGameAsync()
        {
            HttpResponseMessage response = await _client.PutAsync(
                "api/Player/creategame", null);
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }
        /// <summary>
        /// Joins the game and pushes the serverside gamestate to all clients
        /// </summary>
        /// <returns>Http Status Code</returns>
        public async Task<HttpStatusCode> JoinGameAsync()
        {
            HttpResponseMessage response = await _client.PutAsync(
                "api/Player/joingame", null);
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }
        /// <summary>
        /// Login to an existing Player to get the authorization Token
        /// (Http Post)
        /// </summary>
        /// <returns>Http Status Code</returns>
        public async Task<HttpStatusCode> StartGameAsync()
        {
            HttpResponseMessage response = await _client.PutAsync(
                "api/Player/startgame", null);
            response.EnsureSuccessStatusCode();
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
                "api/Player/move", move);
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        public async Task<HttpStatusCode> UpdateAvatar(StringDto path)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync(
                "api/Player/avatar", path);
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        /// <summary>
        /// Tell the server to add an npc
        /// (Http Post)
        /// </summary>
        /// <returns>Http Status Code</returns>
        public async Task<HttpStatusCode> AddNpcAsync()
        {
            HttpResponseMessage response = await _client.PostAsync(
                "api/Player/addnpc", null);
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        /// <summary>
        /// Tell the server to remove a player from the game
        /// </summary>
        /// <param name="username">player name</param>
        /// <returns>StatusCode</returns>
        public async Task<HttpStatusCode> RemoveAsync(string username)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync(
                "api/Player/remove", username);
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
                .WithUrl(_url+ "/gameHub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(_token);
                    options.ApplicationMaxBufferSize = 10240;
                    options.TransportMaxBufferSize = 10240;
                })
                .WithAutomaticReconnect()
                .AddNewtonsoftJsonProtocol(options =>
                {
                    options.PayloadSerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
                    options.PayloadSerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
                })
                .Build();

            connection.On<string>("UpdateGameState", (gameState) =>
            {
                GameState = JsonConvert.DeserializeObject<GameState>(gameState);
                UpdateGameStateEvent(this, EventArgs.Empty);
                newGameStateEvent.Set();
                if (GameState.GameStarted)
                {
                    gameStartedEvent.Set();
                    GameStartedEvent(this, EventArgs.Empty);
                }
            });

            connection.On<string>("GameEnd", (player) =>
            {
                GameEndEventArgs args = new GameEndEventArgs();
                args.Player = player;
                GameEndEvent(this, args);
            });

            connection.ServerTimeout = TimeSpan.FromMinutes(15);


            await connection.StartAsync();
            Console.WriteLine(connection.ConnectionId);
        }
    }
}