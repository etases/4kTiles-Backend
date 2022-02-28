FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["4kTiles-Backend.csproj", "./"]
RUN dotnet restore "4kTiles-Backend.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "4kTiles-Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "4kTiles-Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["ASPNETCORE_URLS=http://*:$PORT", "dotnet", "4kTiles-Backend.dll"]
