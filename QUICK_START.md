# 🦄 Magical Launcher - Quick Start Guide

## One-Click Setup (Windows)

### Step 1: Clone the Repository
```bash
git clone --recursive https://github.com/Halo-tuff/Magical-Launcher.git
cd Magical-Launcher
```

### Step 2: Run Setup
Double-click **`setup.bat`** to automatically:
- Update submodules
- Restore dependencies
- Build the Release version

### Step 3: Launch the App
Double-click **`run.bat`** to launch Magical Launcher! 🦄✨

---

## Alternative: Manual Build

```bash
# Restore dependencies
dotnet restore MagicalLauncher.sln

# Build
dotnet build MagicalLauncher.sln -c Release

# Run
dotnet run --project MagicalLauncher/MagicalLauncher.csproj -c Release
```

---

## Create Standalone Release

Double-click **`build-release.bat`** to create a standalone executable in `./publish/`

Users can then run `MagicalLauncher.exe` directly (requires .NET 6 Runtime)

---

## Requirements

- ✅ **Visual Studio 2022** or **.NET 6 SDK** (https://dotnet.microsoft.com/download/dotnet/6.0)
- ✅ **Windows 10** or newer
- ✅ **.NET 6 Desktop Runtime** (for running the app)
- ✅ **Git** with submodules

---

## Troubleshooting

### Error: "Project files not found"
- Make sure you cloned with `--recursive`
- Run: `git submodule update --init --recursive`

### Error: "dotnet not found"
- Install .NET 6 SDK from https://dotnet.microsoft.com/download/dotnet/6.0

### App won't run
- Install .NET 6 Desktop Runtime
- Check that you built in Release mode

---

## 🎪 You're All Set!

Happy testing with your Magical Launcher! 🦄💫
