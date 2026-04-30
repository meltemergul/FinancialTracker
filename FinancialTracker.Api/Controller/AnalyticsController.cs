using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialTracker.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.Api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;
        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }
        [HttpGet("top-growth")]
        public async Task<IActionResult> TopGrowth([FromQuery] int n = 5, CancellationToken ct = default)
        {
            var result = await _analyticsService.GetTopGrowthAsync(n, ct);
            return Ok(result);
        }
    }
}