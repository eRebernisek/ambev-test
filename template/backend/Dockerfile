FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project files
COPY ["src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj", "src/Ambev.DeveloperEvaluation.WebApi/"]
COPY ["src/Ambev.DeveloperEvaluation.Application/Ambev.DeveloperEvaluation.Application.csproj", "src/Ambev.DeveloperEvaluation.Application/"]
COPY ["src/Ambev.DeveloperEvaluation.Domain/Ambev.DeveloperEvaluation.Domain.csproj", "src/Ambev.DeveloperEvaluation.Domain/"]
COPY ["src/Ambev.DeveloperEvaluation.ORM/Ambev.DeveloperEvaluation.ORM.csproj", "src/Ambev.DeveloperEvaluation.ORM/"]
COPY ["src/Ambev.DeveloperEvaluation.Common/Ambev.DeveloperEvaluation.Common.csproj", "src/Ambev.DeveloperEvaluation.Common/"]
COPY ["src/Ambev.DeveloperEvaluation.IoC/Ambev.DeveloperEvaluation.IoC.csproj", "src/Ambev.DeveloperEvaluation.IoC/"]

# Restore dependencies for the WebApi project
RUN dotnet restore "src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR "/src/src/Ambev.DeveloperEvaluation.WebApi"
RUN dotnet build "Ambev.DeveloperEvaluation.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ambev.DeveloperEvaluation.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ambev.DeveloperEvaluation.WebApi.dll"]
