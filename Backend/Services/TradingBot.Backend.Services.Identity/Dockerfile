#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 1999

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Backend/Services/TradingBot.Backend.Services.Identity/TradingBot.Backend.Services.Identity.Api.csproj", "Backend/Services/TradingBot.Backend.Services.Identity/"]
RUN dotnet restore "Backend/Services/TradingBot.Backend.Services.Identity/TradingBot.Backend.Services.Identity.Api.csproj"
COPY . .
WORKDIR "/src/Backend/Services/TradingBot.Backend.Services.Identity"
RUN dotnet build "TradingBot.Backend.Services.Identity.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TradingBot.Backend.Services.Identity.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TradingBot.Backend.Services.Identity.Api.dll"]