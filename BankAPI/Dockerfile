FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["./BankAPI/BankAPI.csproj", "BankAPI/"]
COPY ["./Messaging/Messaging.csproj", "Messaging/"]
RUN dotnet restore "BankAPI/BankAPI.csproj" -s https://api.nuget.org/v3/index.json
COPY . .
WORKDIR "/src/BankAPI"
RUN dotnet build "BankAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BankAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 5100/tcp
ENV ASPNETCORE_URLS http://*:5100
HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://localhost:5100/hc || exit 1

ENTRYPOINT ["dotnet", "BankAPI.dll"]