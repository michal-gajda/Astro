FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY WebUI/Astro.WebUI.csproj WebUI/Astro.WebUI.csproj
RUN dotnet restore WebUI/Astro.WebUI.csproj

COPY . .

RUN dotnet build WebUI/Astro.WebUI.csproj --configuration Release
RUN dotnet publish WebUI/Astro.WebUI.csproj --configuration Release --output /app/build

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

RUN apt-get update && apt-get install -y curl=7.88.1-10+deb12u12 --no-install-recommends && rm -rf /var/lib/apt/lists/*

RUN groupadd -g 10000 dotnet && useradd -u 10000 -g dotnet dotnet && chown -R dotnet:dotnet /app
USER dotnet:dotnet

ENV ASPNETCORE_HTTP_PORT=
ENV ASPNETCORE_HTTP_PORTS=5080
EXPOSE 5080

COPY --chown=dotnet:dotnet --from=build /app/build .

HEALTHCHECK --interval=5s --timeout=10s --retries=3 CMD curl --fail http://localhost:5080/health || exit

ENTRYPOINT ["dotnet", "Astro.WebUI.dll"]
