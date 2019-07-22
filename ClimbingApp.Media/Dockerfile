FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ClimbingApp.Media/ClimbingApp.Media.csproj ClimbingApp.Media/
RUN dotnet restore ClimbingApp.Media/ClimbingApp.Media.csproj
COPY . .
WORKDIR /src/ClimbingApp.Media
RUN dotnet build ClimbingApp.Media.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ClimbingApp.Media.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ClimbingApp.Media.dll"]
