using Microsoft.EntityFrameworkCore;
using ProductApi.Interface;

namespace ProductApi.Repository
{
    public class ProductRepo(ProductDbContext context) : IProduct
    {
        public async Task<int> AddProduct(Product product)
        {
            context.Product.Add(product);
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var product = await context.Product.FindAsync(id);
            if (product == null)
                return -1;

            context.Product.Remove(product);
            return await context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await context.Product.ToListAsync();
        }

        public async Task<Product> GetAsync(int id)
        {
            return await context.Product.FindAsync(id);
        }

        public async Task<int> UpdateProduct(Product product)
        {
            context.Product.Update(product);
            return await context.SaveChangesAsync();
        }
    }
}
