using NatureBox.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NatureBox.Partners.Service
{
    public interface IPartnerRepository
    {
        Task<IEnumerable<Partner>> GetAllAsync();
        Task<Partner> AddAsync(Partner employee);
        Task<Partner> UpdateAsync(Partner employee);
        Task<Partner> GetEntityByIdAsync(int id);
    }
}
