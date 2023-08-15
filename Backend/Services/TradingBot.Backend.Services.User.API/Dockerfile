#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 2001

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Backend/Services/TradingBot.Backend.Services.User.API/TradingBot.Backend.Services.User.API.csproj", "Backend/Services/TradingBot.Backend.Services.User.API/"]
COPY ["Backend/Libraries/TradingBot.Backend.Libraries.ApiCore/TradingBot.Backend.Libraries.ApiCore.csproj", "Backend/Libraries/TradingBot.Backend.Libraries.ApiCore/"]
COPY ["Backend/Libraries/TradingBot.Backend.Libraries.Infrastructure/TradingBot.Backend.Libraries.Infrastructure.csproj", "Backend/Libraries/TradingBot.Backend.Libraries.Infrastructure/"]
COPY ["Backend/Libraries/TradingBot.Backend.Libraries.Application/TradingBot.Backend.Libraries.Application.csproj", "Backend/Libraries/TradingBot.Backend.Libraries.Application/"]
COPY ["Backend/Libraries/TradingBot.Backend.Libraries.Domain/TradingBot.Backend.Libraries.Domain.csproj", "Backend/Libraries/TradingBot.Backend.Libraries.Domain/"]
COPY ["Backend/Libraries/TradingBot.Backend.Libraries.Persistence/TradingBot.Backend.Libraries.Persistence.csproj", "Backend/Libraries/TradingBot.Backend.Libraries.Persistence/"]
RUN dotnet restore "Backend/Services/TradingBot.Backend.Services.User.API/TradingBot.Backend.Services.User.API.csproj"
COPY . .
WORKDIR "/src/Backend/Services/TradingBot.Backend.Services.User.API"
RUN dotnet build "TradingBot.Backend.Services.User.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TradingBot.Backend.Services.User.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TradingBot.Backend.Services.User.API.dll"]