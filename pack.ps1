Remove-Item .\nupkgs\*.nupkg
& dotnet pack .\FlashElf.ChaosKit --output nupkgs
& nuget pack -Build .\FlashElf.ChaosKit.Autofac\FlashElf.ChaosKit.Autofac.csproj `
    -IncludeReferencedProjects `
    -OutputDirectory nupkgs