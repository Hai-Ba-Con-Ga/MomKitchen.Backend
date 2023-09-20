# Use the official .NET Core SDK image as a build environment
FROM mcr.microsoft.com/dotnet/sdk:7 AS build-env
WORKDIR /app

# Copy the .csproj file and restore dependencies
COPY MK.API/*.csproj ./
RUN dotnet restore

# Copy the remaining source code
COPY . ./
RUN dotnet publish -c Release -o out

# Use a runtime image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:7
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "MK.API.dll"]
