FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["VideoProcessing.API/VideoProcessing.API.csproj", "VideoProcessing.API/"]
RUN dotnet restore "VideoProcessing.API/VideoProcessing.API.csproj"
COPY . .
WORKDIR "/src/VideoProcessing.API"
RUN dotnet publish "VideoProcessing.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "VideoProcessing.API.dll"]
