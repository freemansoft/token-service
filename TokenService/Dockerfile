FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS build
WORKDIR /src
COPY *.sln ./
COPY TokenService/TokenService.csproj TokenService/
COPY Models/Models.csproj Models/
RUN dotnet restore
COPY . .
WORKDIR /src/TokenService
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TokenService.dll"]
