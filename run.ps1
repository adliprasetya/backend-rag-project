param(
  [string]$Action = "start"
)

$Network = "ai-network"
$Postgres = "ai-postgres"
$Api = "ai-api"
$Image = "ai-knowledge-api"

function Ensure-Podman {
  $connected = podman ps 2>$null
  if ($LASTEXITCODE -ne 0) {
    Write-Host "Podman not connected. Starting machine..." -ForegroundColor Yellow
    $machine = podman machine list --format "{{.Name}}" 2>$null | Select-Object -First 1
    if (-not $machine) {
      podman machine init
    }
    podman machine start
    Start-Sleep 3
  }
}

function Start-DbOnly {
  Ensure-Podman
  Write-Host "[1/2] Creating network..." -ForegroundColor Cyan
  podman network create $Network 2>$null

  Write-Host "[2/2] Starting PostgreSQL..." -ForegroundColor Cyan
  $exists = podman ps -a --filter "name=$Postgres" --format "{{.Names}}" 2>$null
  if ($exists) {
    podman start $Postgres
  } else {
    podman run -d --name $Postgres --network $Network `
      -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=ai_knowledge_base `
      -p 5432:5432 pgvector/pgvector:pg17
  }
  Write-Host "`nPostgreSQL ready!" -ForegroundColor Green
  Write-Host "  Host: localhost:5432" -ForegroundColor Yellow
  Write-Host "  DB:   ai_knowledge_base" -ForegroundColor Yellow
  Write-Host "`nNow run 'dotnet run' from src/Api/" -ForegroundColor Cyan
}

function Start-Containers {
  Ensure-Podman

  Write-Host "[1/4] Building API image..." -ForegroundColor Cyan
  podman build -t $Image -f src/Api/Dockerfile .

  Write-Host "[2/4] Creating network..." -ForegroundColor Cyan
  podman network create $Network 2>$null

  Write-Host "[3/4] Starting PostgreSQL..." -ForegroundColor Cyan
  $exists = podman ps -a --filter "name=$Postgres" --format "{{.Names}}" 2>$null
  if ($exists) {
    podman start $Postgres
  } else {
    podman run -d --name $Postgres --network $Network `
      -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=ai_knowledge_base `
      -p 5432:5432 pgvector/pgvector:pg17
  }

  Write-Host "[4/4] Starting API..." -ForegroundColor Cyan
  podman run -d --name $Api --network $Network -p 8081:8080 `
    -e "ConnectionStrings__Default=Host=$Postgres;Database=ai_knowledge_base;Username=postgres;Password=postgres" `
    -e ASPNETCORE_ENVIRONMENT=Development `
    $Image

  Write-Host "`nAll containers started!" -ForegroundColor Green
  Write-Host "  API:     http://localhost:8081" -ForegroundColor Yellow
  Write-Host "  Swagger: http://localhost:8081/swagger" -ForegroundColor Yellow
  Write-Host "  Health:  http://localhost:8081/api/health" -ForegroundColor Yellow
}

function Stop-Containers {
  Write-Host "Stopping containers..." -ForegroundColor Cyan
  podman stop $Api $Postgres 2>$null
  podman rm $Api $Postgres 2>$null
  podman network rm $Network -f 2>$null
  Write-Host "All stopped." -ForegroundColor Green
}

switch ($Action) {
  "start" { Start-Containers }
  "stop"  { Stop-Containers }
  "db"    { Start-DbOnly }
  default { Write-Host "Usage: ./run.ps1 start|stop|db" -ForegroundColor Red }
}
