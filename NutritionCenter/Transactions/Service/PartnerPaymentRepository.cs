using NatureBox.DataAccess;
using NatureBox.Model;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using NatureBox.Service;

namespace NatureBox.Transactions.Service
{
    public class PartnerPaymentRepository : IPartnerPaymentRepository
    {
        private readonly Func<NatureBoxDbContext> _context;

        public PartnerPaymentRepository(Func<NatureBoxDbContext> contextCreator)
        {
            _context = contextCreator;
        }

        public async Task<List<PartnerPayment>> GetAllAsync()
        {
            using (var ctx = _context())
            {
                return await ctx.PartnerPayments.AsNoTracking().ToListAsync();
            }
        }

        public async Task<PartnerPayment> AddAsync(PartnerPayment partnerPayment)
        {
            using (var ctx = _context())
            {
                ctx.PartnerPayments.Add(partnerPayment);

                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                }

                return partnerPayment;
            }
        }
    }
}

