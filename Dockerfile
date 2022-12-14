#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

RUN mkdir -p "/data/reports/"

RUN apt-get update && apt-get install -y --allow-unauthenticated\
    libc6-dev \
    libgdiplus \
    libx11-dev \
	&& rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TelerikDocker.csproj", "."]
RUN dotnet restore "./TelerikDocker.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "TelerikDocker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TelerikDocker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TelerikDocker.dll"]