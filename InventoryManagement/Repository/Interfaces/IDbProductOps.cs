using InventoryManagement.Models;

namespace InventoryManagement.Repository.Interfaces
{
    public interface IDbProductOps
    {
        /// <summary>
        /// Saves data uploaded from excel file.
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        Task<int> SaveProductsFromExcel(List<Product> products);
        /// <summary>
        /// Gets all products from database
        /// </summary>
        /// <returns></returns>
        Task<List<Product>> GetAllProducts();
        /// <summary>
        /// Add the the product into database
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task AddProduct(Product product);
        /// <summary>
        /// Gets the product with specified productId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<Product> GetProductById(int productId);
        /// <summary>
        /// Updates the specified product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<int> UpdateProduct(Product product);
        /// <summary>
        /// Deleted product with specified product Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<int> DeleteProduct(int productId);
    }
}
