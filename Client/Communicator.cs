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

namespace InspectorGoe
{

    public class Communicator
    {
        private HttpClient _client; 

        public Communicator()
        {
            _client = new HttpClient();

        }

        public void initClient(String url)
        {
            _client.BaseAddress = new Uri(url);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

        }
        //todo: private token...
        public void setToken(String token)
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<HttpStatusCode> CreatePlayerAsync(Player player)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync(
                "api/Player", player);
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        public async Task<String> Login(Player player)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync(
                "api/Player/login", player);
            response.EnsureSuccessStatusCode();
            String tokenJson = await response.Content.ReadAsStringAsync();
            String token = System.Text.Json.JsonSerializer.Deserialize<Token>(
                tokenJson).token;
            return token;
        }

        public async Task<HttpStatusCode> MovePlayerAsync(MovePlayerDto move)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync(
                "api/Player", move);
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        public async Task<Player> GetPlayer()
        {
            HttpResponseMessage response = await _client.GetAsync("api/Player");
            response.EnsureSuccessStatusCode();
            String playerJson = await response.Content.ReadAsStringAsync();
            Player player = System.Text.Json.JsonSerializer.Deserialize<Player>(
                playerJson);
            return player;
        }

        public async Task RunAsyncSignalR(String token)
        {
            HubConnection connection;
            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5000/gameHub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token);
                })
                .WithAutomaticReconnect()
                .Build();

            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Console.WriteLine($"{message}");
            });

            connection.On<GameState>("ReceiveGameState", (gameState) =>
            {
                Console.WriteLine($"Runde: {gameState.Round}");
            });

            await connection.StartAsync();



            while (true) { }
        }
    }
}