using NatureBox.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NatureBox.Transactions.Service
{
    public interface ICustomerPaymentRepository
    {
        Task<CustomerPayment> AddAsync(CustomerPayment customer);
        Task DeleteAsync(int paymentId);
        Task<List<CustomerPayment>> GetAllAsync();
        Task<CustomerPayment> GetCustomerAsync(int id);
        Task<CustomerPayment> UpdateAsync(CustomerPayment payment);
    }
}