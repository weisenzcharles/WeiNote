#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Swagger_v2/Swagger_v2.csproj", "Swagger_v2/"]
RUN dotnet restore "Swagger_v2/Swagger_v2.csproj"
COPY . .
WORKDIR "/src/Swagger_v2"
RUN dotnet build "Swagger_v2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Swagger_v2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Swagger_v2.dll"]