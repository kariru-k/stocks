using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stock
{
    public class StockRequestDTO
    {
        [Required]
        [MaxLength(length: 10, ErrorMessage = "Symbols cannot be over 10 characters")]
        public string Symbol { get; set; } = string.Empty;
        [Required]
        [MinLength(length: 100, ErrorMessage = "Company name cannot be over 10 characters")]
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        public decimal Purchase {get; set;}
        [Required]
        public decimal LastDiv { get; set; }
        [Required]
        public string Industry { get; set; } = string.Empty;
        [Required]
        public long MarketCap { get; set; }
    }
}