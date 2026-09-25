FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore src/D6_UNIFOR_ACHADOS_PERDIDOS_API.WebApi/D6_UNIFOR_ACHADOS_PERDIDOS_API.WebApi.csproj
RUN dotnet publish src/D6_UNIFOR_ACHADOS_PERDIDOS_API.WebApi/D6_UNIFOR_ACHADOS_PERDIDOS_API.WebApi.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "D6_UNIFOR_ACHADOS_PERDIDOS_API.WebApi.dll"]