using InventoryManagement.DataAccess;
using InventoryManagement.Models;
using InventoryManagement.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Repository.Implementations
{
    public class DbProductOperations : IDbProductOps
    {
        private readonly AppDbContext _context;
        public DbProductOperations(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task AddProduct(Product product)
        {
            try
            {
                if (product is not null)
                {
                    await _context.Products.AddAsync(product);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<int> DeleteProduct(int productId)
        {
            try
            {
                var product=await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
                if (product is not null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    return 1;
                }
                return 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            return products;
        }

        public async Task<Product> GetProductById(int productId)
        {
            var product = await _context.Products.SingleOrDefaultAsync(p => p.Id == productId);
            return product;
        }

        public async Task<int> SaveProductsFromExcel(List<Product> products)
        {
            try
            {
                await _context.Products.AddRangeAsync(products);
                var response= await _context.SaveChangesAsync();
                if (response>0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<int> UpdateProduct(Product product)
        {
            int successful = 0;
            if (product is not null)
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                successful = 1;
            }
            return successful;

        }
    }
}
