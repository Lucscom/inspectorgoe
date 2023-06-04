using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GameComponents.Model;

namespace InspectorGoe
{

    class Communicator
    {
        static HttpClient client = new HttpClient();

        static void ShowProduct(Player product)
        {
            Console.WriteLine($"Name: {product.Name}\tPrice: " +
                $"{product.BusTicket}\tCategory: {product.BikeTicket}");
        }

        static async Task<Uri> CreateProductAsync(Player product)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/Player", product);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
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


        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("http://localhost:5014/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Create a new Player
                PointOfInterest poi = new PointOfInterest(1, System.Numerics.Vector2.One);
                Player player1 = new Player(poi);



                var url = await CreateProductAsync(player1);
                Console.WriteLine($"Created at {url}");

                //// update the product
                //console.writeline("updating price...");
                //product.price = 80;
                //await updateproductasync(product);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}