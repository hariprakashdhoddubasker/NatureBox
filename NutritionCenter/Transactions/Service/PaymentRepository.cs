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
    public class PaymentRepository : ICustomerPaymentRepository
    {
        private readonly Func<NatureBoxDbContext> _context;

        public PaymentRepository(Func<NatureBoxDbContext> contextCreator)
        {
            _context = contextCreator;
        }

        public async Task<List<CustomerPayment>> GetAllAsync()
        {
            using (var ctx = _context())
            {
                return await ctx.CustomerPayments.AsNoTracking().ToListAsync();
            }
        }

        public Task<CustomerPayment> GetCustomerAsync(int id)
        {
            using (var ctx = _context())
            {
                return ctx.CustomerPayments.FirstOrDefaultAsync(c => c.CustomerId == id);
            }
        }

        public async Task<CustomerPayment> AddAsync(CustomerPayment customerPayment)
        {
            using (var ctx = _context())
            {
                ctx.CustomerPayments.Add(customerPayment);
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                }
                return customerPayment;
            }
        }

        public async Task<CustomerPayment> UpdateAsync(CustomerPayment payment)
        {
            using (var ctx = _context())
            {
                if (!ctx.CustomerPayments.Local.Any(c => c.PaymentId == payment.PaymentId))
                {
                    ctx.CustomerPayments.Attach(payment);
                }
                ctx.Entry(payment).State = EntityState.Modified;
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                }
                return payment;
            }

        }

        public async Task DeleteAsync(int paymentId)
        {
            using (var ctx = _context())
            {
                var payment = ctx.CustomerPayments.FirstOrDefault(c => c.PaymentId == paymentId);

                if (payment != null)
                {
                    ctx.CustomerPayments.Remove(payment);
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
