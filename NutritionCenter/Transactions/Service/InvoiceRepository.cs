using Microsoft.EntityFrameworkCore;
using NatureBox.DataAccess;
using NatureBox.Model;
using NatureBox.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NatureBox.Transactions.Service
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly Func<NatureBoxDbContext> _context;

        public InvoiceRepository(Func<NatureBoxDbContext> contextCreator)
        {
            _context = contextCreator;
        }

        public async Task<List<Invoice>> GetAllAsync()
        {
            using (var ctx = _context())
            {
                return await ctx.Invoices.AsNoTracking().ToListAsync();
            }
        }

        public async Task<Invoice> AddAsync(Invoice invoice)
        {
            using (var ctx = _context())
            {
                ctx.Invoices.Add(invoice);

                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                }

                return invoice;
            }
        }

        public async Task<List<Invoice>> UpdateRangeAsync(List<Invoice> invoices)
        {
            using (var ctx = _context())
            {
                if (!ctx.Invoices.Local.Except(invoices).Any())
                {
                    ctx.Invoices.AttachRange(invoices);
                }
                foreach (var invoice in invoices)
                {
                    ctx.Entry(invoice).State = EntityState.Modified;
                }
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                }
                return invoices;
            }
        }

        public async Task<bool> DeleteAsync(int invoiceId)
        {
            using (var ctx = _context())
            {
                var invoice = ctx.Invoices.FirstOrDefault(c => c.InvoiceId == invoiceId);

                if (invoice != null)
                {
                    ctx.Invoices.Remove(invoice);
                }
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                }
                return true;
            }
        }
    }
}
