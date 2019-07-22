FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ClimbingApp.Routes/ClimbingApp.Routes.csproj ClimbingApp.Routes/
RUN dotnet restore ClimbingApp.Routes/ClimbingApp.Routes.csproj
COPY . .
WORKDIR /src/ClimbingApp.Routes
RUN dotnet build ClimbingApp.Routes.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ClimbingApp.Routes.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ClimbingApp.Routes.dll"]
