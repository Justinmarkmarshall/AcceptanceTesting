using System;
using Microsoft.AspNetCore.Mvc;

namespace Product.Api.Contracts.Responses
{
    public class ProductResponse
    {
        public Guid Id { get; init; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal RRP { get; set; }

        public bool IsMarkdown { get; set; }

        public string CurrencyIsoCode { get; set; }

        public string CategoryName { get; set; }
    }
}
