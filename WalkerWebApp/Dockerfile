# Use the official .NET runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the official .NET SDK as the build image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WalkerWebApp.csproj", "./"]
RUN dotnet restore "./WalkerWebApp.csproj"
COPY . .
RUN dotnet publish "WalkerWebApp.csproj" -c Release -o /app/publish

# Final image stage
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WalkerWebApp.dll"]