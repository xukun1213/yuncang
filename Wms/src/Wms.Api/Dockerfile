#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Wms.Api/Wms.Api.csproj", "src/Wms.Api/"]
RUN dotnet restore "src/Wms.Api/Wms.Api.csproj"
COPY . .
WORKDIR "/src/src/Wms.Api"
RUN dotnet build "Wms.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Wms.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wms.Api.dll"]