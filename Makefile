build:
	dotnet build src/WebUI/Astro.WebUI.csproj

docker:
	docker compose build

down:
	docker compose down

publish:
	dotnet publish src/WebUI/Astro.WebUI.csproj --configuration Release --output ./app

run:
	dotnet run --project src/WebUI/Astro.WebUI.csproj

up:
	docker compose up -d
