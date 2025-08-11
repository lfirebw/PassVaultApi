# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

WORKDIR /src

COPY PasswordListing.sln ./
COPY PasswordListing/*.csproj PasswordListing/
COPY PasswordListing.Application/*.csproj PasswordListing.Application/
COPY PasswordListing.Domain/*.csproj PasswordListing.Domain/
COPY PasswordListing.Infrastructure/*.csproj PasswordListing.Infrastructure/
RUN dotnet restore

COPY . .
WORKDIR /src/PasswordListing
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final

WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 5063
ENTRYPOINT ["dotnet", "PasswordListing.dll"]