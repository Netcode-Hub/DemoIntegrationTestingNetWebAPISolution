namespace ProductApi.Services
{
    public class ProductService(HttpClient client)
    {
        public async Task<List<Product>> GetProducts()
        {
            try
            {
                var products = await client.GetFromJsonAsync<List<Product>>("http://localhost:7000/api/products");
                return products!;
            }
            catch
            {
                return [];
            }
        }
    }
}
