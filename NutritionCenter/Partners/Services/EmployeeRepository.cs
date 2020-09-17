namespace NatureBox.Partners.Service
{
    using NatureBox.DataAccess;
    using NatureBox.Model;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    using System;
    using NatureBox.Service;

    public class EmployeeRepository : IPartnerRepository
    {
        private readonly Func<NatureBoxDbContext> _context;

        public EmployeeRepository(Func<NatureBoxDbContext> contextCreator)
        {
            _context = contextCreator;
        }    

        public async Task<IEnumerable<Partner>> GetAllAsync()
        {
            using (var ctx = _context())
            {
                return await ctx.Employees.AsNoTracking().ToListAsync();
            }
        }

        public Task<Partner> GetEntityByIdAsync(int id)
        {
            using (var ctx = _context())
            {
                return ctx.Employees.FirstOrDefaultAsync(c => c.EmployeeId == id);
            }
        }

        public async Task<Partner> AddAsync(Partner employee)
        {
            using (var ctx = _context())
            {
                ctx.Employees.Add(employee);
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                }
                return employee;
            }
        }

        public async Task<Partner> UpdateAsync(Partner employee)
        {
            using (var ctx = _context())
            {
                if (!ctx.Employees.Local.Any(c => c.EmployeeId == employee.EmployeeId))
                {
                    ctx.Employees.Attach(employee);
                }
                ctx.Entry(employee).State = EntityState.Modified;
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                }
                return employee;
            }

        }
    }
}
