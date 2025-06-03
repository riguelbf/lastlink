# backend/Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore ./app.sln
RUN dotnet build ./app.sln -c Release --no-restore
RUN dotnet publish ./Presentation/Presentation.csproj -c Release -o /app/publish --no-build

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5053
ENV ASPNETCORE_URLS=http://0.0.0.0:5053
ENTRYPOINT ["dotnet", "Presentation.dll"]
