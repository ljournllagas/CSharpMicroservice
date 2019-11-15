FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["./ClientService/ClientService.csproj", "ClientService/"]
COPY ["./Messaging/Messaging.csproj", "Messaging/"]
RUN dotnet restore "ClientService/ClientService.csproj" -s https://api.nuget.org/v3/index.json
COPY . .
WORKDIR "/src/ClientService"
RUN dotnet build "ClientService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClientService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ClientService.dll"]