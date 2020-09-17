using NatureBox.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NatureBox.Customers.Service
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllAsync();
        Task<Customer> GetCustomerAsync(int id);
        Task<Customer> AddAsync(Customer customer);
        Task<Customer> UpdateAsync(Customer customer);
        Task DeleteAsync(int customerId);
    }
}