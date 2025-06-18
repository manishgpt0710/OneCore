FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /app
COPY . ./
RUN dotnet restore
RUN dotnet build
RUN dotnet publish -c Release src/OneCore --no-restore

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
ENV ASPNETCORE_HTTP_PORTS=80
WORKDIR /app
COPY --from=build /app/src/OneCore/bin/Release/net8.0/publish ./
EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "OneCore.dll"]
