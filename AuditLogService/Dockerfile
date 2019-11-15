FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["./AuditLogService/AuditLogService.csproj", "AuditLogService/"]
COPY ["./Messaging/Messaging.csproj", "Messaging/"]
RUN dotnet restore "AuditLogService/AuditLogService.csproj" -s https://api.nuget.org/v3/index.json
COPY . .
WORKDIR "/src/AuditLogService"
RUN dotnet build "AuditLogService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuditLogService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "AuditLogService.dll"]