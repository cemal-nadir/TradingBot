#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 2023

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Frontend/Web/TradingBot.Frontend.Web.Blazor/TradingBot.Frontend.Web.Blazor.csproj", "Frontend/Web/TradingBot.Frontend.Web.Blazor/"]
COPY ["Frontend/Libraries/TradingBot.Frontend.Libraries.Blazor/TradingBot.Frontend.Libraries.Blazor.csproj", "Frontend/Libraries/TradingBot.Frontend.Libraries.Blazor/"]
RUN dotnet restore "Frontend/Web/TradingBot.Frontend.Web.Blazor/TradingBot.Frontend.Web.Blazor.csproj"
COPY . .
WORKDIR "/src/Frontend/Web/TradingBot.Frontend.Web.Blazor"
RUN dotnet build "TradingBot.Frontend.Web.Blazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TradingBot.Frontend.Web.Blazor.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TradingBot.Frontend.Web.Blazor.dll"]