# Use official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy everything and restore dependencies
COPY . ./
RUN dotnet restore

# Ensure the Images directory exists
RUN mkdir -p /app/Images

# Build and publish the app
RUN dotnet publish -c Release -o /out

# Use a lightweight .NET runtime image for production
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /out ./

# Ensure Images folder exists in the runtime container
RUN mkdir -p /app/Images

# Expose the port
EXPOSE 8080

# Start the application
ENTRYPOINT ["dotnet", "CodePulse.API.dll"]
