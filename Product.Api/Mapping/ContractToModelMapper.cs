using Product.Api.Contracts.Requests;
using Product.Api.Contracts.Responses;
using Product.Api.Models;

namespace Product.Api.Mapping
{
    public static class ContractToModelMapper
    {
        public static Models.Product MapToProduct(this CreateProductRequest request)
        {
            return new()
            {
                Name = request.Name,
                Description = request.Description,
                Category = (ProductCategory)request.Category,
                Price = request.Price,
                RRP = request.RRP,
                CurrencyIsoCode = request.CurrencyIsoCode
            };
        }

        public static ProductResponse MapToProductResponse(this Models.Product product)
        {
            return new()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                RRP = product.RRP,
                CategoryName = product.Category.ToString("G"),
                IsMarkdown = product.Price < product.RRP,
                CurrencyIsoCode = product.CurrencyIsoCode
            };
        }
    }
}