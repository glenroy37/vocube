#----------------- Build Image
FROM mcr.microsoft.com/dotnet/sdk:5.0.101-buster-slim-amd64 AS build
WORKDIR /src
COPY . .
RUN dotnet publish "Vocube.csproj" -c Release -o /app/publish

#----------------- App Image
FROM mcr.microsoft.com/dotnet/aspnet:5.0.1-buster-slim-amd64 AS base
ENV ASPNETCORE_ENVIRONMENT Docker
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build /app/publish .
COPY ./docker-entrypoint.sh /app/docker-entrypoint.sh
ENTRYPOINT ["sh", "/app/docker-entrypoint.sh"]