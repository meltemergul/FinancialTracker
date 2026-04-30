FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["FinancialTracker.Api/FinancialTracker.Api.csproj", "FinancialTracker.Api/"]
RUN dotnet restore "FinancialTracker.Api/FinancialTracker.Api.csproj"

COPY . .
WORKDIR "/src/FinancialTracker.Api"
RUN dotnet publish "FinancialTracker.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "FinancialTracker.Api.dll"]
