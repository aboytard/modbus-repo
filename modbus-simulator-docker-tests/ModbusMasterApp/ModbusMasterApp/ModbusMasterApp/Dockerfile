# Use the official .NET SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

# Set the working directory inside the container
WORKDIR /app

# Copy the client source code into the container
COPY . ./

# Add the NModbus package using dotnet add package
RUN dotnet add package NModbus --version 3.0.78

# Build the client application
RUN dotnet publish -c Release -o out

# Use a smaller runtime image for the final image
FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY --from=build-env /app/out .

# Set environment variables to specify the host's IP address and port
ENV SERVER_HOST=host.docker.internal
ENV SERVER_PORT=12177

# Run the client application
ENTRYPOINT ["dotnet", "ModbusMasterApp.dll"]