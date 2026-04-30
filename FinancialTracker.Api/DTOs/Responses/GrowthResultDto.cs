namespace FinancialTracker.Api.DTOs.Responses
{
    public class GrowthResultDto
    {
        public string Symbol { get; set; } = string.Empty;
        public decimal CurrentPrice { get; set; }
        public decimal PreviousClose { get; set; }
        public decimal GrowthPercent { get; set; }
    }
}