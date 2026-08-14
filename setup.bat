@echo off
REM 🦄 Magical Launcher Quick Setup Script
echo.
echo ============================================
echo    🦄 Magical Launcher Setup 🦄
echo ============================================
echo.

echo [1/4] Updating submodules...
git submodule update --init --recursive

echo.
echo [2/4] Restoring NuGet packages...
dotnet restore MagicalLauncher.sln

echo.
echo [3/4] Building Release version...
dotnet build MagicalLauncher.sln -c Release

echo.
echo [4/4] Build complete!
echo.
echo ============================================
echo    ✨ Setup Complete! ✨
echo ============================================
echo.
echo You can now:
echo  - Open MagicalLauncher.sln in Visual Studio
echo  - Run: dotnet run --project MagicalLauncher/MagicalLauncher.csproj -c Release
echo.
pause