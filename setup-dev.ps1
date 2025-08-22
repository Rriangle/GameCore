# ===============================
# GameCore Project Initialization Script
# Steps: Deploy → Initialize Database → Run Tests
# ===============================

# Stop execution if any error occurs
$ErrorActionPreference = "Stop"

function Run-Step($stepName, $scriptBlock) {
    try {
        Write-Host ">>> Starting: $stepName ..." -ForegroundColor Green
        & $scriptBlock
        Write-Host ">>> Completed: $stepName" -ForegroundColor Cyan
        Write-Host ""
    }
    catch {
        Write-Host "!!! ERROR in step: $stepName" -ForegroundColor Red
        Write-Host $_.Exception.Message -ForegroundColor Red
        exit 1
    }
}

# -------------------------------
# Step 1: Deploy
# -------------------------------
Run-Step "Setup development environment and start website" {
    .\scripts\Deploy-GameCore.ps1 -Environment Development -DeploymentType Local
    dotnet run --project GameCore.Web
}

# -------------------------------
# Step 2: Database Initialization
# -------------------------------
Run-Step "Initialize database" {
    Push-Location Tools
    .\DataMigrationTool.ps1 -Action Init
    .\DataMigrationTool.ps1 -Action SeedMassive
    Pop-Location
}

# -------------------------------
# Step 3: Tests
# -------------------------------
Run-Step "Run tests" {
    .\run-tests.ps1 -TestType All -Coverage
}

Write-Host "=== Project initialization process completed successfully! ===" -ForegroundColor Yellow
