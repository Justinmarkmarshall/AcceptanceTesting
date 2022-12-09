using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product.Api.Data;

namespace Product.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        public async Task<Models.Product> CreateAsync(Models.Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Models.Product> GetAsync(Guid productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public async Task<IReadOnlyList<Models.Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task DeleteAsync(Guid productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product is null) return;
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
