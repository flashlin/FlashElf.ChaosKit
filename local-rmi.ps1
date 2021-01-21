& docker images --format "{{json .}}" | ConvertFrom-Json | 
Where-Object {
    $_.Repository -eq '<none>'
} | ForEach-Object {
    Write-Host "$($_.Repository)"
    & docker rmi $_.ID -f
}

& docker rmi chaos-website -f