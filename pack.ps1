Remove-Item .\nupkgs\*.nupkg
& dotnet pack .\FlashElf.ChaosKit --output nupkgs

# & nuget pack -Build .\FlashElf.ChaosKit.Autofac\FlashElf.ChaosKit.Autofac.csproj `
#     -IncludeReferencedProjects `
#     -OutputDirectory nupkgs

$msbuild1 = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\amd64\MSBuild.exe"
$msbuild2 = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\amd64\MSBuild.exe"

$msbuild = $msbuild2
if( [System.IO.File]::Exists($msbuild1) ) {
    $msbuild = $msbuild1
}

& $msbuild .\FlashElf.ChaosKit.Autofac\FlashElf.ChaosKit.Autofac.csproj `
    -t:pack