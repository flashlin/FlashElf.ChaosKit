$apiKeyFile = "$env:USERPROFILE/flashElf.apiKey"
$apiKey = [IO.File]::ReadAllText($apiKeyFile)

function GetVerInfo {
    param(
        [string]$ver
    )
    $rc = $ver -match "(?<m>\d+)\.(?<n>\d+)\.(?<b>\d+)"
    if ( $rc -eq $false) {
        throw "Not parse version number"
    }
    $major = [int]$Matches["m"]
    $minor = [int]$Matches["n"]
    $revision = [int]$Matches["b"]
    $revision += 1
    if ( $revision -ge 100 ) {
        $revision = 0
        $minor += 1
    }
    if ( $minor -ge 100 ) {
        $minor = 0
        $major += 1
    }
    return @{
        Major = $major
        Minor = $minor
        Revision = $revision
    }
}

function GetNupkgName {
    param(
        [string]$nupkgFileName
    )
    $rc = $nupkgFileName -match "(?<ver>\d+\.\d+\.\d+)"
    if ( $rc -eq $false) {
        throw "Can't get version string from '$nupkgFileName'"
    }
    $ver = $Matches["ver"] 
    $idx = $nupkgFileName.IndexOf($ver)
    $nupkgName = $nupkgFileName.Substring(0, $idx - 1)
    return @{
        FileName = $nupkgFileName
        Name = $nupkgName
        Version = $ver
        Ver = GetVerInfo $ver
    } 
}

function GetLastNugetPackage {
    param(
        [string]$packagename
    )
    nuget list $packagename | Where-Object {
        $_ -match "^$packagename "
    } | ForEach-Object {
        $text = $_
        $name = $text.Substring(0, "$packagename ".Length)
        $version = $text.Substring($name.Length)
        @{
            Name = $name
            Version = $version
            Ver  = GetVerInfo $version
        }
    }
}

#$package = GetLastNugetPackage "FlashElf.ChaosKit"
#Write-Host $package.Ver.Major

$files = Get-ChildItem .\nupkgs\*.nupkg | ForEach-Object {
    $nupkgFileName = $_.Name
    Write-Host "Checking $nupkgFileName online version..."
    GetNupkgName $nupkgFileName 
} | Where-Object {
    $nupkg = $_
    $nugetLast = GetLastNugetPackage $nupkg.Name
    $nupkg.Version -ne $nugetLast.Version
}

$files | ForEach-Object {
    $nupkg = $_
    $file = "./nupkgs/$($nupkg.FileName)"
    Write-Host "Deploy $($nupkg.FileName)"
    nuget push $file -Source https://api.nuget.org/v3/index.json -apiKey $apiKey
}
