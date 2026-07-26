# 🦄 Building & Testing Magical Launcher

## Prerequisites

1. **Visual Studio 2022** or **Visual Studio Code** with C# extensions
2. **.NET 6 SDK** - Download from [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet/6.0)
3. **Git** with submodules support

## Quick Start - Build & Run

### Option 1: Using Visual Studio (Easiest)

1. Open `MagicalLauncher.sln` in Visual Studio 2022
2. Right-click on the **MagicalLauncher** project → Set as Startup Project
3. Press **F5** or click **Run** to build and launch
4. The app will compile and start automatically!

### Option 2: Using Command Line

```bash
# Clone the repository with submodules
git clone --recursive https://github.com/Halo-tuff/Magical-Launcher.git
cd Magical-Launcher

# Restore dependencies
dotnet restore MagicalLauncher.sln

# Build in Release mode
dotnet build MagicalLauncher.sln -c Release

# Run the application
dotnet run --project MagicalLauncher/MagicalLauncher.csproj -c Release
```

### Option 3: Build Standalone Executable

```bash
# Publish as a self-contained executable
dotnet publish MagicalLauncher/MagicalLauncher.csproj -c Release -o ./publish

# The executable will be at: ./publish/MagicalLauncher.exe
```

## Installation Requirements

- **Windows 10** or newer (64-bit)
- **.NET 6 Desktop Runtime** ([Download](https://aka.ms/dotnet-core-applaunch?missing_runtime=true&arch=x64&rid=win10-x64&apphost_version=6.0.0&gui=true))

## Troubleshooting

### "The runtime is missing" error
- Install .NET 6 Desktop Runtime from the link above

### "Submodules not found"
- Run: `git submodule update --init --recursive`

### Build fails with "Project files not found"
- Ensure you cloned with: `git clone --recursive`

## 🎪 Testing the Magical Launcher

Once built, you can:
1. Launch `MagicalLauncher.exe` directly
2. Test the installer functionality
3. Check Discord Rich Presence (if configured)
4. Verify Roblox integration works correctly

## 🦄 Want to Contribute?

Feel free to:
- Report issues
- Submit pull requests
- Suggest magical new features

---

✨ **Happy coding with your Magical Launcher!** ✨
