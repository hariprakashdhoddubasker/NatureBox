using Microsoft.EntityFrameworkCore;
using NatureBox.DataAccess;
using NatureBox.Model;
using NatureBox.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NatureBox.Customers.Service
{
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly Func<NatureBoxDbContext> _context;

        public HealthRecordRepository(Func<NatureBoxDbContext> contextCreator)
        {
            _context = contextCreator;
        }

        public async Task<List<HealthRecord>> GetAllAsync()
        {
            using (var ctx = _context())
            {
                return await ctx.HealthRecords.AsNoTracking().ToListAsync();
            }
        }

        public Task<HealthRecord> GetEntityByCustomerIdAsync(int customerId)
        {
            using (var ctx = _context())
            {
                return ctx.HealthRecords.FirstOrDefaultAsync(c => c.CustomerId == customerId);
            }
        }


        public async Task<HealthRecord> AddAsync(HealthRecord healthRecord)
        {
            using (var ctx = _context())
            {
                ctx.HealthRecords.Attach(healthRecord);
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                }

                return healthRecord;
            }
        }

        public async Task<IEnumerable<HealthRecord>> AddRangeAsync(IEnumerable<HealthRecord> healthRecords)
        {
            using (var ctx = _context())
            {
                ctx.HealthRecords.AttachRange(healthRecords);
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                }

                return healthRecords;
            }
        }

        public async Task<HealthRecord> UpdateAsync(HealthRecord healthRecord)
        {
            using (var ctx = _context())
            {
                if (!ctx.HealthRecords.Local.Any(c => c.HealthRecordId == healthRecord.HealthRecordId))
                {
                    ctx.HealthRecords.Attach(healthRecord);
                }
                ctx.Entry(healthRecord).State = EntityState.Modified;
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                }
                return healthRecord;
            }

        }

        public async Task DeleteAsync(int healthRecordId)
        {
            using (var ctx = _context())
            {
                var healthRecord = ctx.HealthRecords.FirstOrDefault(c => c.HealthRecordId == healthRecordId);
                if (healthRecord != null)
                {
                    ctx.HealthRecords.Remove(healthRecord);
                }
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                }
            }
        }
    }

}
