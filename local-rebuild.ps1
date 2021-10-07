docker rm $(docker ps -a -q)
docker rmi chaos-website
docker build -t chaos-website -f ChaosSiteSample/Dockerfile .