using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GameComponents.Model;

namespace HttpClientSample
{

    class Program
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

        static async Task<Player> GetProductAsync(string path)
        {
            Player product = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                //product = await response.Content.ReadAsAsync<Product>();
            }
            return product;
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

        static async Task<HttpStatusCode> DeleteProductAsync(string id)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                $"api/products/{id}");
            return response.StatusCode;
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

                //// get the product
                //product = await getproductasync(url.pathandquery);
                //showproduct(product);

                //// update the product
                //console.writeline("updating price...");
                //product.price = 80;
                //await updateproductasync(product);

                //// get the updated product
                //product = await getproductasync(url.pathandquery);
                //showproduct(product);

                //// delete the product
                //var statuscode = await deleteproductasync(product.id);
                //console.writeline($"deleted (http status = {(int)statuscode})");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}