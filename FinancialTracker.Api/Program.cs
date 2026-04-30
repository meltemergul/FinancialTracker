using FinancialTracker.Api.Clients;
using FinancialTracker.Api.Clients.Interfaces;
using FinancialTracker.Api.Data;
using FinancialTracker.Api.Middleware;
using FinancialTracker.Api.Repositories;
using FinancialTracker.Api.Repositories.Interfaces;
using FinancialTracker.Api.Services;
using FinancialTracker.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IPriceSnapshotRepository, PriceSnapshotRepository>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddHttpClient<IFinanceApiClient, FinnhubApiClient>(client =>
{
    client.BaseAddress = new Uri("https://finnhub.io/api/v1/");
});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();