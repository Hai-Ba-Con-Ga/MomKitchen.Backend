# Use the official .NET Core SDK image as a build environment
FROM bitnami/dotnet-sdk:7.0.102 AS build-env
WORKDIR /app

# Copy the .csproj file and restore dependencies
COPY MK.API/*.csproj ./
RUN dotnet restore

# Copy the remaining source code
COPY . ./
RUN dotnet publish -c Release -o out

# Use a runtime image for the final container
FROM bitnami/dotnet-sdk:7.0.102
WORKDIR /app
COPY --from=build-env /app/out .
COPY --from=build-env /app/MK.API/*.json ./

EXPOSE 5000

ENTRYPOINT ["dotnet", "MK.API.dll"]
