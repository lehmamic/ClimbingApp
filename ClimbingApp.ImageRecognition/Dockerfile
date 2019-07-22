FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ClimbingApp.ImageRecognition/ClimbingApp.ImageRecognition.csproj ClimbingApp.ImageRecognition/
RUN dotnet restore ClimbingApp.ImageRecognition/ClimbingApp.ImageRecognition.csproj
COPY . .
WORKDIR /src/ClimbingApp.ImageRecognition
RUN dotnet build ClimbingApp.ImageRecognition.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ClimbingApp.ImageRecognition.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ClimbingApp.ImageRecognition.dll"]
