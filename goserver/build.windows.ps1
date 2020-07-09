$Image = "morcodou/psmandelgo:nanoserver-1803"

docker build -f Dockerfile.windows -t $Image .

docker push $Image