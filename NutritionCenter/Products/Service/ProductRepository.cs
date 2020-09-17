using NatureBox.DataAccess;
using NatureBox.Model;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using NatureBox.Service;

namespace NatureBox.Products.Service
{
    public class ProductRepository : IProductRepository
    {
        private readonly Func<NatureBoxDbContext> _context;

        public ProductRepository(Func<NatureBoxDbContext> contextCreator)
        {
            _context = contextCreator;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            using (var ctx = _context())
            {
                return await ctx.Products.AsNoTracking().ToListAsync();
            }
        }

        public Task<Product> GetEntityByIdAsync(int id)
        {
            using (var ctx = _context())
            {
                return ctx.Products.FirstOrDefaultAsync(c => c.ProductId == id);
            }
        }

        public async Task<Product> AddAsync(Product product)
        {
            using (var ctx = _context())
            {
                ctx.Products.Attach(product);
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                }
                return product;
            }
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            using (var ctx = _context())
            {
                if (!ctx.Products.Local.Any(c => c.ProductId == product.ProductId))
                {
                    ctx.Products.Attach(product);
                }
                ctx.Entry(product).State = EntityState.Modified;
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    LogService.LogException(ex);
                }
                return product;
            }
        }

        public async Task DeleteAsync(int productId)
        {
            using (var ctx = _context())
            {
                var product = ctx.Products.FirstOrDefault(c => c.ProductId == productId);
                if (product != null)
                {
                    ctx.Products.Remove(product);
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
