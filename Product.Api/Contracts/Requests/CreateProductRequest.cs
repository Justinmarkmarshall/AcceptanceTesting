using Microsoft.AspNetCore.Mvc;

namespace Product.Api.Contracts.Requests
{
    public class CreateProductRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal RRP { get; set; }

        public string CurrencyIsoCode { get; set; }

        public int Category { get; set; }
    }
}
