@echo off
REM 🦄 Build standalone release executable
echo.
echo ============================================
echo    Building Standalone Release
echo ============================================
echo.

echo Publishing to ./publish directory...
dotnet publish MagicalLauncher/MagicalLauncher.csproj -c Release -o ./publish

echo.
echo ============================================
echo    ✨ Release Build Complete! ✨
echo ============================================
echo.
echo Executable location:
echo    ./publish/MagicalLauncher.exe
echo.
echo Requirements for users:
echo    - Windows 10 or newer
echo    - .NET 6 Desktop Runtime
echo.
pause