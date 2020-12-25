Remove-Item .\nupkgs\*.nupkg
& dotnet pack .\FlashElf.ChaosKit --output nupkgs

# & nuget pack -Build .\FlashElf.ChaosKit.Autofac\FlashElf.ChaosKit.Autofac.csproj `
#     -IncludeReferencedProjects `
#     -OutputDirectory nupkgs

$msbuild = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\amd64\MSBuild.exe"
& $msbuild .\FlashElf.ChaosKit.Autofac\FlashElf.ChaosKit.Autofac.csproj `
    -t:pack