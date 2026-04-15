FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY WebUI/Astro.WebUI.csproj WebUI/Astro.WebUI.csproj
RUN dotnet restore WebUI/Astro.WebUI.csproj

COPY . .

RUN dotnet build WebUI/Astro.WebUI.csproj --configuration Release
RUN dotnet publish WebUI/Astro.WebUI.csproj --configuration Release --output /app/build

FROM mcr.microsoft.com/dotnet/aspnet:10.0-azurelinux3.0
WORKDIR /app

RUN tdnf install -y shadow-utils; groupadd -g 10000 dotnet; useradd -u 10000 -g 10000 -s /sbin/nologin -M dotnet; tdnf clean all;
USER dotnet:dotnet

COPY --chown=dotnet:dotnet --from=build /app/build .

ENV ASPNETCORE_HTTP_PORTS=5080
EXPOSE 5080

HEALTHCHECK --interval=5s --timeout=10s --retries=3 CMD curl --fail http://localhost:5080/healthz || exit 1

ENTRYPOINT ["dotnet", "Astro.WebUI.dll"]
