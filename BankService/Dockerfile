FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["./BankService/BankService.csproj", "BankService/"]
COPY ["./Messaging/Messaging.csproj", "Messaging/"]
RUN dotnet restore "BankService/BankService.csproj" -s https://api.nuget.org/v3/index.json
COPY . .
WORKDIR "/src/BankService"
RUN dotnet build "BankService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BankService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "BankService.dll"]