using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Servicios
{
    public class TestAPIWebService
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<string> GetProducts()
        {
            try
            {
                var response = await client.GetAsync("https://fakestoreapi.com/products");

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();
                
                return "Error: No se pudo obtener la información de la API.";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
