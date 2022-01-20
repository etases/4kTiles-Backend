#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["4kTiles-Backend/4kTiles-Backend.csproj", "4kTiles-Backend/"]
RUN dotnet restore "4kTiles-Backend/4kTiles-Backend.csproj"
COPY . .
WORKDIR "/src/4kTiles-Backend"
RUN dotnet build "4kTiles-Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "4kTiles-Backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "4kTiles-Backend.dll"]