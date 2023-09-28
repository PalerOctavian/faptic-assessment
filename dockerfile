FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FapticService/FapticService.csproj", "FapticService/"]
COPY ["FapticService.API/FapticService.API.csproj", "FapticService.API/"]
COPY ["FapticService.Business/FapticService.Business.csproj", "FapticService.Business/"]
COPY ["FapticService.Common/FapticService.Common.csproj", "FapticService.Common/"]
COPY ["FapticService.Domain/FapticService.Domain.csproj", "FapticService.Domain/"]
COPY ["FapticService.EntityFramework/FapticService.EntityFramework.csproj", "FapticService.EntityFramework/"]
RUN dotnet restore "FapticService/FapticService.csproj"
COPY . .
WORKDIR "/src/FapticService"
RUN dotnet build "FapticService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FapticService.csproj" -c Release -o /app/publish

FROM base AS final

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FapticService.dll"]
