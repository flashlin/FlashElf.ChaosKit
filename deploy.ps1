$apiKeyFile = "$env:USERPROFILE/flashElf.apiKey"

$apiKey = [IO.File]::ReadAllText($apiKeyFile)

nuget push .\nupkgs\*.nupkg -Source https://api.nuget.org/v3/index.json -apiKey $apiKey