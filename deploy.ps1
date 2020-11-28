$name = Read-Host "What is your github name?" 
$pass = Read-Host 'What is your password?' -AsSecureString

$p = [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($pass))
nuget push .\nupkgs\*.nupkg -Source https://api.nuget.org/v3/index.json -username $name -password $p