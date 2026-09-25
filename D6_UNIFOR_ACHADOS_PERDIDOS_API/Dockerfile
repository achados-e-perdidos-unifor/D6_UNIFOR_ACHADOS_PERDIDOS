FROM mcr.microsoft.com/dotnet/sdk:10.0@sha256:35d40304542c8689331f8cab17c65926cdf48fe711e289321d71924b230a7d29 AS build
WORKDIR /src

COPY src/D6_UNIFOR_ACHADOS_PERDIDOS_API.Domain/*.csproj src/D6_UNIFOR_ACHADOS_PERDIDOS_API.Domain/
COPY src/D6_UNIFOR_ACHADOS_PERDIDOS_API.Application/*.csproj src/D6_UNIFOR_ACHADOS_PERDIDOS_API.Application/
COPY src/D6_UNIFOR_ACHADOS_PERDIDOS_API.Infrastructure/*.csproj src/D6_UNIFOR_ACHADOS_PERDIDOS_API.Infrastructure/
COPY src/D6_UNIFOR_ACHADOS_PERDIDOS_API.WebApi/*.csproj src/D6_UNIFOR_ACHADOS_PERDIDOS_API.WebApi/
RUN dotnet restore src/D6_UNIFOR_ACHADOS_PERDIDOS_API.WebApi/D6_UNIFOR_ACHADOS_PERDIDOS_API.WebApi.csproj
COPY . .
RUN dotnet publish src/D6_UNIFOR_ACHADOS_PERDIDOS_API.WebApi/D6_UNIFOR_ACHADOS_PERDIDOS_API.WebApi.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0@sha256:2d584d8147faddb0d678c5748d47953e5b8e18621ed4fb7049a91381d9d7746f AS final
WORKDIR /app

RUN apt-get update \
    && apt-get install --no-install-recommends -y curl \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build --chown=1654:1654 /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
USER $APP_UID

HEALTHCHECK --interval=30s --timeout=3s --start-period=10s --retries=3 \
    CMD curl --fail --silent http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "D6_UNIFOR_ACHADOS_PERDIDOS_API.WebApi.dll"]