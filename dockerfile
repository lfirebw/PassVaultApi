# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS Development

WORKDIR /app

COPY PasswordListing.sln ./
COPY PasswordListing/*.csproj PasswordListing/
COPY PasswordListing.Application/*.csproj PasswordListing.Application/
COPY PasswordListing.Domain/*.csproj PasswordListing.Domain/
COPY PasswordListing.Infrastructure/*.csproj PasswordListing.Infrastructure/
COPY PasswordListing.Tests/*.csproj PasswordListing.Tests/

RUN dotnet restore

COPY . .

EXPOSE 5063

# ENV DOTNET_USE_POLLING_FILE_WATCHER=1
# ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_HOST_PATH=/usr/share/dotnet/dotnet