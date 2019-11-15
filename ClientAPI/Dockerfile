FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["./ClientAPI/ClientAPI.csproj", "ClientAPI/"]
COPY ["./Messaging/Messaging.csproj", "Messaging/"]
RUN dotnet restore "ClientAPI/ClientAPI.csproj" -s https://api.nuget.org/v3/index.json
COPY . .
WORKDIR "/src/ClientAPI"
RUN dotnet build "ClientAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClientAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000
HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://localhost:5000/hc || exit 1

ENTRYPOINT ["dotnet", "ClientAPI.dll"]