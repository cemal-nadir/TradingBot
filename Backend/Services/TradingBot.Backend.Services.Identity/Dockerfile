#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Backend/Services/TradingBot.Backend.Services.Identity/TradingBot.Backend.Services.Identity.Api.csproj", "Backend/Services/TradingBot.Backend.Services.Identity/"]
RUN dotnet restore "./Backend/Services/TradingBot.Backend.Services.Identity/TradingBot.Backend.Services.Identity.Api.csproj"
COPY . .
WORKDIR "/src/Backend/Services/TradingBot.Backend.Services.Identity"
RUN dotnet build "./TradingBot.Backend.Services.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TradingBot.Backend.Services.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TradingBot.Backend.Services.Identity.Api.dll"]