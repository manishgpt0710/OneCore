# Build Angular app
Write-Host "Building Angular app..."
Set-Location -Path "src/OneCore.UI"
npm run build:prod
Set-Location -Path "../../"

# Create wwwroot directory if it doesn't exist
if (-not (Test-Path "src/OneCore/wwwroot")) {
    New-Item -ItemType Directory -Path "src/OneCore/wwwroot"
} else {
    Remove-Item -Path "src/OneCore/wwwroot/*" -Recurse -Force
}

# Copy Angular build output to wwwroot
Write-Host "Copying Angular build output to wwwroot..."
Copy-Item -Path "src/OneCore.UI/dist/OneCore.UI/browser/*" -Destination "src/OneCore/wwwroot" -Recurse -Force

# Build and publish .NET Core app
Write-Host "Building and publishing .NET Core app..."
dotnet publish src/OneCore/OneCore.csproj -c Release -o publish
