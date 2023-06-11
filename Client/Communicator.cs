using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GameComponents.Model;

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
            String token = await response.Content.ReadAsStringAsync();
            return token;
        }

        static async Task<Player> UpdateProductAsync(Player product)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"api/products/{product.Id}", product);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            //product = await response.Content.ReadAsAsync<Product>();
            return product;
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
                //var code = await CreatePlayerAsync(player1);

                var token = await Login(player1);
                Console.WriteLine(token);


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