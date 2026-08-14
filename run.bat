@echo off
REM 🦄 Quick run script for Magical Launcher
echo.
echo 🦄 Launching Magical Launcher...
echo.

if exist "MagicalLauncher\bin\Release\net6.0-windows\MagicalLauncher.exe" (
    start "" "MagicalLauncher\bin\Release\net6.0-windows\MagicalLauncher.exe"
    echo ✨ Magical Launcher launched!
) else (
    echo ⚠️  Build not found. Running setup first...
    call setup.bat
    start "" "MagicalLauncher\bin\Release\net6.0-windows\MagicalLauncher.exe"
)