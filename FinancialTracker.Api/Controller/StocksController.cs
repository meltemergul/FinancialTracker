using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialTracker.Api.DTOs.Requests;
using FinancialTracker.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace FinancialTracker.Api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly IStockService _stockService;
        public StocksController(IStockService stockService)
        {
            _stockService = stockService;
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddStockRequest request, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(request.Symbol))
                return BadRequest("Symbol is required.");
            var result = await _stockService.AddStockAsync(request.Symbol, ct);
            return CreatedAtAction(nameof(GetAll), new { symbol = result.Symbol }, result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await _stockService.GetAllAsync(ct);
            return Ok(result);
        }
        [HttpPost("{symbol}/refresh")]
        public async Task<IActionResult> Refresh(string symbol, CancellationToken ct)
        {
            await _stockService.RefreshAsync(symbol, ct);
            return NoContent();
        }
        [HttpDelete("{symbol}")]
        public async Task<IActionResult> Delete(string symbol, CancellationToken ct)
        {
            await _stockService.DeleteAsync(symbol, ct);
            return NoContent();
        }
    }
}