FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ClimbingApp.ApiGateway/ClimbingApp.ApiGateway.csproj ClimbingApp.ApiGateway/
RUN dotnet restore ClimbingApp.ApiGateway/ClimbingApp.ApiGateway.csproj
COPY . .
WORKDIR /src/ClimbingApp.ApiGateway
RUN dotnet build ClimbingApp.ApiGateway.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ClimbingApp.ApiGateway.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ClimbingApp.ApiGateway.dll"]
