using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace Product.Api.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();

        public string Name { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "decimal(19,4)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(19,4)")]
        public decimal RRP { get; set; }

        public string CurrencyIsoCode { get; set; }

        public ProductCategory Category { get; set; }
    }
}
