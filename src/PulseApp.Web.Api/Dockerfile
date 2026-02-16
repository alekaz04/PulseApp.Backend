FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/PulseApp.Web.Api/PulseApp.Web.Api.csproj", "src/PulseApp.Web.Api/"]
COPY ["src/Infrastructure/PulseApp.Logging/PulseApp.Logging.csproj", "src/Infrastructure/PulseApp.Logging/"]
COPY ["src/Common/PulseApp.Common/PulseApp.Common.csproj", "src/Common/PulseApp.Common/"]
COPY ["src/Common/PulseApp.Application/PulseApp.Application.csproj", "src/Common/PulseApp.Application/"]
COPY ["src/Infrastructure/PulseApp.Infrastructure/PulseApp.Infrastructure.csproj", "src/Infrastructure/PulseApp.Infrastructure/"]
COPY ["src/Common/PulseApp.Domain/PulseApp.Domain.csproj", "src/Common/PulseApp.Domain/"]
COPY ["src/Infrastructure/PulseApp.Hangfire/PulseApp.Hangfire.csproj", "src/Infrastructure/PulseApp.Hangfire/"]
COPY ["src/Common/PulseApp.Authentication/PulseApp.Authentication.csproj", "src/Common/PulseApp.Authentication/"]
RUN dotnet restore "src/PulseApp.Web.Api/PulseApp.Web.Api.csproj"
COPY . .
WORKDIR "/src/src/PulseApp.Web.Api"
RUN dotnet build "./PulseApp.Web.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PulseApp.Web.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PulseApp.Web.Api.dll"]
