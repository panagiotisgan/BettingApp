#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80/tcp
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Betting.API/Betting.API.csproj", "Betting.API/"]
COPY ["Betting.DataAccess/Betting.DataAccess.csproj", "Betting.DataAccess/"]
COPY ["Betting.Domain/Betting.Domain.csproj", "Betting.Domain/"]
RUN dotnet restore "Betting.API/Betting.API.csproj"
COPY . .
WORKDIR "/src/Betting.API"
RUN dotnet build "Betting.API.csproj" -c Release -o /app/build
#RUN chmod +x ./entrypoint.sh
#CMD /bin/bash ./entrypoint.sh

FROM build AS publish
RUN dotnet publish "Betting.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Betting.API.dll"]