#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Greatmarch.Beer.API.Auth/Greatmarch.Beer.API.Auth.csproj", "Greatmarch.Beer.API.Auth/"]
RUN dotnet restore "Greatmarch.Beer.API.Auth/Greatmarch.Beer.API.Auth.csproj"
COPY . .
WORKDIR "/src/Greatmarch.Beer.API.Auth"
RUN dotnet build "Greatmarch.Beer.API.Auth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Greatmarch.Beer.API.Auth.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Greatmarch.Beer.API.Auth.dll"]