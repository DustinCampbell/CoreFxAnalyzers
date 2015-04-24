$CurrentPath = $(Get-Location)
$BinariesPath = Join-Path $CurrentPath "Binaries\Release"
$PackagePath = Join-Path $BinariesPath "Package"
$ToolsPath = Join-Path $PackagePath "tools"
$AnalyzersPath = Join-Path $ToolsPath "analyzers"
$NuGet = Join-Path $CurrentPath "Tools\NuGet\NuGet.exe"
$Solution = Join-Path $CurrentPath "CoreFxAnalyzers.sln"
$NuSpec = Join-Path $CurrentPath "CoreFxAnalyzers.nuspec"
$DllName = "CoreFxAnalyzers.dll"
$DllPath = Join-Path $BinariesPath "CoreFxAnalyzers"
$DllPath = Join-Path $DllPath $DllName

# Perform NuGet package restore
Invoke-Expression "$NuGet restore $Solution"
""

# Build solution with MSBuild in Release
Invoke-Expression "msbuild.exe /m $Solution /p:Configuration=Release"
""

# Create the package path. If it already exists, delete the old package path first.
"Preparing files for packaging..."

if ((Test-Path $PackagePath) -eq $True)
{
    Remove-Item -Recurse -Force $PackagePath
}

New-Item $PackagePath -Type Directory | Out-Null

"  * Created `"Package`" directory"

New-Item $ToolsPath -Type Directory | Out-Null

"  * Created `"tools`" directory"

New-Item $AnalyzersPath -Type Directory | Out-Null

"  * Created `"tools\analyzers`" directory"

Copy-Item -Path $DllPath -Destination (Join-Path $AnalyzersPath $DllName)

"  * Copied `"$DllName`" to `"tools\analyzers`" directory"

Copy-Item -Path (Join-Path $CurrentPath "install.ps1") -Destination (Join-Path $ToolsPath "install.ps1")

"  * Copied `"install.ps1`" to `"tools`" directory"

Copy-Item -Path (Join-Path $CurrentPath "uninstall.ps1") -Destination (Join-Path $ToolsPath "uninstall.ps1")

"  * Copied `"uninstall.ps1`" to `"tools`" directory"

""

# Package NuGet
Invoke-Expression "$NuGet pack $NuSpec -BasePath $PackagePath -OutputDirectory $PackagePath"