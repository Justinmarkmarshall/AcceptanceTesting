using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace Product.Api.Models
{
    public enum ProductCategory
    {
        Uncategorised = -1,
        Clothing,
        Homeware
    }
}
