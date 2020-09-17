using NatureBox.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NatureBox.Transactions.Service
{
    public interface IPartnerPaymentRepository
    {
        Task<PartnerPayment> AddAsync(PartnerPayment partnerPayment);
        Task<List<PartnerPayment>> GetAllAsync();
    }
}