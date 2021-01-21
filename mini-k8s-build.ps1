#minikube docker-env --shell powershell
& minikube docker-env --shell powershell | Invoke-Expression
& docker build -t chaos-website -f ChaosSiteSample/Dockerfile .
