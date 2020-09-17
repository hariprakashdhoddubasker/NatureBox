using NatureBox.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NatureBox.Products.Service
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product product);
        Task DeleteAsync(int customerId);
        Task<List<Product>> GetAllAsync();
        Task<Product> GetEntityByIdAsync(int id);
        Task<Product> UpdateAsync(Product product);
    }
}