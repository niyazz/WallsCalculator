FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
WORKDIR /src
ARG AppName=WallsCalculator

COPY . ${AppName}/
RUN dotnet restore ${AppName}/${AppName}.csproj
COPY . .
WORKDIR /src/${AppName}
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:5.0 as final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "WallsCalculator.dll"]

