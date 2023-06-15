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
        static HttpClient client = new HttpClient();

        /// <summary>
        /// Create a new Player on the Server with username and password
        /// </summary>
        /// <param name="player">Player Object</param>
        /// <returns>Http Status code</returns>
        static async Task<HttpStatusCode> CreatePlayerAsync(Player player)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/Player", player);
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        /// <summary>
        /// Login a Player, that is already registered on the server
        /// </summary>
        /// <param name="player">Player Object</param>
        /// <returns>bearer token to authenticate on  the server</returns>
        static async Task<String> Login(Player player)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/Player/login", player);
            response.EnsureSuccessStatusCode();
            String tokenJson = await response.Content.ReadAsStringAsync();
            String token = System.Text.Json.JsonSerializer.Deserialize<Token>(
                tokenJson).token;
            return token;
        }

        /// <summary>
        /// Sends the move to the server
        /// </summary>
        /// <param name="move">MovePlayerDto Object with POI and Ticket</param>
        /// <returns>Http Status Code</returns>
        static async Task<HttpStatusCode> MovePlayerAsync(MovePlayerDto move)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                "api/Player", move);
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        public async static Task Main()
        {
            await RunAsync();
        }

        static async Task RunAsync()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("https://localhost:5000/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Create a new Player
                Player player1 = new Player(Name:"Henri", pw:"1234");
                // Register Player on the Server
                var code = await CreatePlayerAsync(player1);
                // Login Player to receive token
                var token = await Login(player1);
                Console.WriteLine(token);
                // add the token to the http client authorization header 
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
                // create move object
                var vector = new System.Numerics.Vector2(0, 0);
                var poi = new PointOfInterest(1, "test", vector);
                var ticket = TicketTypeEnum.Bike;
                var move = new MovePlayerDto(poi, ticket);

                await RunAsyncSignalR(token);

                //var code2 = MovePlayerAsync(move);
                //Console.WriteLine(code2);
                //var player = GetPlayer();
                //Console.WriteLine(player);
                //var url = await CreateProductAsync(player1);
                //Console.WriteLine($"Created at {url}");

                //// update the product
                //console.writeline("updating price...");
                //product.price = 80;
                //await updateproductasync(product);

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }

            Console.ReadLine();
        }

        static async Task RunAsyncSocket()
        {

            try
            {

                using (var ws = new ClientWebSocket())
                {
                    await ws.ConnectAsync(new Uri("wss://localhost:5000/ws"), CancellationToken.None);
                    var buffer = new byte[256];
                    while (ws.State == WebSocketState.Open)
                    {
                        var result = await ws.ReceiveAsync(buffer, CancellationToken.None);
                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
                        }
                        else
                        {
                            Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, result.Count));
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }

        /// <summary>
        /// Initializes hub connection to server with token as authentication
        /// </summary>
        /// <param name="token">Used for authentication</param>
        /// <returns></returns>
        static async Task RunAsyncSignalR(String token)
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

            //Register method that can be called from the server
            connection.On<GameState>("ReceiveGameState", (gameState) =>
            {
                Console.WriteLine($"Runde: {gameState.Round}");
            });

            await connection.StartAsync();


            //TODO: remove
            while (true) { }
        }
    }
}