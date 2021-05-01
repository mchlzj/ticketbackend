FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore -v diag

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "DockerAPI.dll"]