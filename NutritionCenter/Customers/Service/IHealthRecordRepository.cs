using NatureBox.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NatureBox.Customers.Service
{
    public interface IHealthRecordRepository
    {
        Task<HealthRecord> AddAsync(HealthRecord healthRecord);
        Task<IEnumerable<HealthRecord>> AddRangeAsync(IEnumerable<HealthRecord> healthRecords);
        Task DeleteAsync(int healthRecordId);
        Task<List<HealthRecord>> GetAllAsync();
        Task<HealthRecord> GetEntityByCustomerIdAsync(int customerId);
        Task<HealthRecord> UpdateAsync(HealthRecord healthRecord);
    }
}