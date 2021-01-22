param(
    [Parameter(Mandatory=$false)]
    [string]$action
)

if( "" -eq $action ) {
    Write-Host ""
    return
}

#& docker rmi chaos-website -f
function Rmi() {
    & docker rmi chaos-website -f
}

function Build() {
    & docker build -t chaos-website -f ChaosSiteSample/Dockerfile .
}

function Deploy() {
    & kubectl apply -f kubernetes-manifest/staging.deployment.yaml
}

function ReDeploy() { 
    & kubectl delete -f kubernetes-manifest/staging.deployment.yaml
    Deploy
}

function Info {
    param(
        [string] $message
    )
    Write-Host $message -ForegroundColor Green -BackgroundColor Black
}

Info "Initialize docker-env ..."

& minikube docker-env --shell powershell | Invoke-Expression

if( "re-deploy" -eq $action ) {
    ReDeploy
}

if( "build" -eq $action ) {
    Rmi
    Build
}

if( "deploy" -eq $action ) {
    Info "Deploy" 
    Deploy
}

#kubectl get services
& kubectl get all
Write-Host "minikube dashboard"
#minikube service chaos-website --url
