kubectl delete -f kubernetes-manifest/staging.deployment.yaml
kubectl apply -f kubernetes-manifest/staging.deployment.yaml
kubectl get services
kubectl get all
Write-Host "minikube dashboard"
minikube service chaos-website --url
