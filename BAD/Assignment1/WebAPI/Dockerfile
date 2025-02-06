# ---------------------------------------------
# STAGE 1: Build and Publish
# ---------------------------------------------
    FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
    WORKDIR /app
    
    # Copy the full solution and restore only the Assignment1 project
    COPY . ./
    RUN dotnet restore "./WebAPI.csproj"
    
    # Build and publish to a folder called "out"
    RUN dotnet publish "./WebAPI.csproj" -c Release -o /app/out
    
    # ---------------------------------------------
    # STAGE 2: Runtime
    # ---------------------------------------------
    FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
    WORKDIR /app
    
    # Copy the compiled output from the build stage
    COPY --from=build /app/out ./
    
    # Expose port 80 and set ASP.NET Core to listen on it
    ENV ASPNETCORE_URLS=http://+:80
    EXPOSE 80
    
    # Run the application
    ENTRYPOINT ["dotnet", "WebAPI.dll"]
    