# smartfinance-server

/////////////////////////////
run local version of server
/////////////////////////////

dotnet run -c debug

/////////////////////////////
navigate docker on server
/////////////////////////////

//show logs
docker logs smartfinance-server

//show all containers
docker ps -a

//show all networks
docker network ls

//enter docker container (in this case db)
docker exec -it dev-mysql mysql

docker exec -it dev-mysql bash

mysql -u dev -p (see appsettings.json)

one command from root:
docker exec -it dev-mysql -u dev -p
/////////////////////////////
release new version of server
/////////////////////////////

// build deployable version of smartfinance-server (change version number accordingly)
docker build -t circularcode/smartfinance-server:0.1.6 .

// push to dockerhub
docker push circularcode/smartfinance-server:0.1.6

// stop running container
docker stop smartfinance-server

// remove container
docker rm smartfinance-server

// download from docker hub and run (add -d for passive, or close putty)
docker run --network smartfinance-net --name smartfinance-server -p 8080:80 circularcode/smartfinance-server:0.1.6
