using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialTracker.Api.Models
{
    public class PriceSnapshot
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal PreviousClose { get; set; }
        public DateTime FetchedAtUtc { get; set; } = DateTime.UtcNow;
        public Stock Stock { get; set; } = null!;
    }
}