FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OTel.Example.Web/OTel.Example.Web.csproj", "OTel.Example.Web/"]
COPY ["OTel.Example.ServiceDefaults/OTel.Example.ServiceDefaults.csproj", "OTel.Example.ServiceDefaults/"]
RUN dotnet restore "OTel.Example.Web/OTel.Example.Web.csproj"
COPY . .
WORKDIR "/src/OTel.Example.Web"
RUN dotnet build "OTel.Example.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OTel.Example.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OTel.Example.Web.dll"]
