FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS base
EXPOSE 12345
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["server/server.csproj", "server/"]
COPY ["shared/shared.csproj", "shared/"]
RUN dotnet restore "server/server.csproj"
COPY . .
WORKDIR "/src/server"
RUN dotnet build "server.csproj" -c Debug -o /app

FROM build AS publish
RUN dotnet publish "server.csproj" -c Debug -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "server.dll"]