# backend/Dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy solution files
COPY ./*.sln .
COPY ./*/*.csproj ./

# Restore NuGet packages
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet build -c Release --no-restore

# Publish the app
WORKDIR /app/Presentation
RUN dotnet publish -c Release -o out --no-restore --no-build

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Install culture-specific packages for globalization
RUN apt-get update && apt-get install -y --no-install-recommends \
    libgdiplus \
    libc6-dev \
    && rm -rf /var/lib/apt/lists/*

# Copy the published app
COPY --from=build /app/Presentation/out .

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://0.0.0.0:5053
ENV TZ=UTC

# Expose the port the app runs on
EXPOSE 5053

# Set the entry point
ENTRYPOINT ["dotnet", "Presentation.dll"]
