using NatureBox.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NatureBox.Transactions.Service
{
    public interface IInvoiceRepository
    {
        Task<Invoice> AddAsync(Invoice invoice);
        Task<List<Invoice>> GetAllAsync();
        Task<List<Invoice>> UpdateRangeAsync(List<Invoice> invoices);
        Task<bool> DeleteAsync(int invoiceId);
    }
}