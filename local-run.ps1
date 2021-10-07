docker stop $(docker ps -a -q)
docker rm $(docker ps -a -q)
docker run -it --name chaos1 -p 10080:80 chaos-website