using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GameComponents.Model;
using GameComponents;

namespace InspectorGoe
{

    public class Communicator
    {
        static HttpClient client = new HttpClient();

        static void ShowProduct(Player product)
        {
            Console.WriteLine($"Name: {product.UserName}\tPrice: " +
                $"{product.BusTicket}\tCategory: {product.BikeTicket}");
        }

        static async Task<HttpStatusCode> CreatePlayerAsync(Player player)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/Player", player);
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

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

        static async Task<HttpStatusCode> MovePlayerAsync(MovePlayerDto move)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                "api/Player", move);
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }

        static async Task<Player> GetPlayer()
        {
            HttpResponseMessage response = await client.GetAsync("api/Player");
            response.EnsureSuccessStatusCode();
            String playerJson = await response.Content.ReadAsStringAsync();
            Player player = System.Text.Json.JsonSerializer.Deserialize<Player>(
                playerJson);
            return player;
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
                var code = await CreatePlayerAsync(player1);

                var token = await Login(player1);
                Console.WriteLine(token);
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
                var vector = new System.Numerics.Vector2(0, 0);
                var poi = new PointOfInterest(1, "test", vector);
                var ticket = TicketTypeEnum.Bike;
                var move = new MovePlayerDto(poi, ticket);
                //var code2 = MovePlayerAsync(move);
                //Console.WriteLine(code2);
                var player = GetPlayer();
                Console.WriteLine(player);
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
    }
}