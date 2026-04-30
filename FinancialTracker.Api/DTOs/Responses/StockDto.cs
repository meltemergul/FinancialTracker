using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialTracker.Api.DTOs.Responses
{
    public class StockDto
    {
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public decimal? LatestPrice { get; set; }
        public DateTime? LastFetchedAtUtc { get; set; }
    }
}