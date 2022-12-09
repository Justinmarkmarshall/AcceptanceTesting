using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Product.Api.Services
{
    public interface IProductService
    {
        Task<Models.Product> CreateAsync(Models.Product product);

        Task<Models.Product> GetAsync(Guid productId);

        Task<IReadOnlyList<Models.Product>> GetAllAsync();

        Task DeleteAsync(Guid productId);
    }
}
