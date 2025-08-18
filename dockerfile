# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS Development

WORKDIR /src

COPY PasswordListing.sln ./
COPY PasswordListing/*.csproj PasswordListing/
COPY PasswordListing.Application/*.csproj PasswordListing.Application/
COPY PasswordListing.Domain/*.csproj PasswordListing.Domain/
COPY PasswordListing.Infrastructure/*.csproj PasswordListing.Infrastructure/
RUN dotnet restore

COPY . .

RUN dotnet watch --project PasswordListing run --urls http://+:5063