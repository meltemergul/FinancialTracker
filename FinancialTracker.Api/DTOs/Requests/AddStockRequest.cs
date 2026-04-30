using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialTracker.Api.DTOs.Requests
{
    public class AddStockRequest
    {
        public string Symbol { get; set; } = string.Empty;
    }
}