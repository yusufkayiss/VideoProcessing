FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["VideoProcessing.Worker/VideoProcessing.Worker.csproj", "VideoProcessing.Worker/"]
RUN dotnet restore "VideoProcessing.Worker/VideoProcessing.Worker.csproj"
COPY . .
WORKDIR "/src/VideoProcessing.Worker"
RUN dotnet publish "VideoProcessing.Worker.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "VideoProcessing.Worker.dll"]
