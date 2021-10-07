#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 50050

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ChaosSiteSample/ChaosSiteSample.csproj", "ChaosSiteSample/"]
COPY ["FlashElf.ChaosKit/FlashElf.ChaosKit.csproj", "FlashElf.ChaosKit/"]
RUN dotnet restore "ChaosSiteSample/ChaosSiteSample.csproj"
COPY . .
WORKDIR "/src/ChaosSiteSample"
RUN dotnet build "ChaosSiteSample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChaosSiteSample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChaosSiteSample.dll"]