using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Numerics;
using System.Threading.Tasks;
using GameComponents.Model;

namespace InspectorGoe
{

    public class Communicator
    {
        private static HttpClient client = new HttpClient();
        private static Uri baseAddress = new Uri("https://localhost:5000/");

        public static async Task<Player> RegisterNewPlayerAsync(Player player)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                baseAddress + "api/Player", player);
            response.EnsureSuccessStatusCode();

            //URI of the created resource.
            var uri = response.Headers.Location;
            // Deserialize the updated player from the response body.
            var updatedPlayer = await response.Content.ReadFromJsonAsync<Player>();
            return updatedPlayer;
        }


        public static async Task MovePlayer(MovePlayerDto move)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                baseAddress + "api/player", move);
            response.EnsureSuccessStatusCode();
        }


        public static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        public static async Task RunAsync()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("http://localhost:5000/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Create a new Player
                PointOfInterest poi = new PointOfInterest(1, "TEST LOCATION", Vector2.One);
                var newPlayer = new Player(poi);
                newPlayer.UserName = "TestUser1";
                newPlayer.Password = "TestPassword";
                var registeredPlayer = await RegisterNewPlayerAsync(newPlayer);
                Console.WriteLine($"Created player object with id: {registeredPlayer.Id}");
                // Move the player
                //this will not work since the player is not authenticated
                //todo: we need to add authentication to the client
                //also the poi probably does not exist
                //todo: choose a poi that exists on the server
                await MovePlayer(new MovePlayerDto(poi, GameComponents.TicketTypeEnum.Bus));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}