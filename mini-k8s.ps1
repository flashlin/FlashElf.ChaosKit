docker build -t chaos-website -f ChaosSiteSample/Dockerfile .
kubectl apply -f KubernetesManifest/staging.deployment.yaml
kubectl get services