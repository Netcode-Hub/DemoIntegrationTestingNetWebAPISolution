namespace ProductApi.Interface
{
    public interface IProduct
    {
        Task<int> AddProduct(Product product);
        Task<Product> GetAsync(int id);
        Task<List<Product>> GetAllAsync();
        Task<int> DeleteAsync(int id);
        Task<int> UpdateProduct(Product product);
    }
}
