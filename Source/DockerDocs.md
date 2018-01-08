Docker and Docker-Compose are powerful tools but they have a very steep learning curve. Each container is really meant to only have one function and to gain the full benefit of the ecosystem, it's really important
to learn both. For a full tutorial on docker and docker compose, see the links in the reference section. 

# Notes

## Dockerfile vs Docker-Compose.yml

Dockerfile is an unrealized definition of how the system will eventually create a Docker image. Docker images are files that go through stages to complete a functional system, and then get saved out as an Image. Think of them as templates.
To realize a Docker image into an actual container, you have to either use a docker run command to create a container or something like a docker-compose along with a configuration file to connect the image to an actual running system container. 

## Visual Studio and Docker
Visual Studio has pretty decent tooling around docker support, and you can even debug from a docker container that is hosting your code
if you so desire. Adding docker support to a project is as simple as right clicking on the project and "add docker support". This adds a Dockerfile to your project
and a new project called docker-compose. 

Right clicking on the docker-compose.yml will give you a debug option in the context menu and from here is where you start debugging your application.

I found it more useful though to run things from the command line, however both options are available.

## Docker and Docker Compose from the command line

For running from the command line, "Terminal Window" under "other windows" in vs.net is your best friend. I personally use this as well as an external Powershell window to compose and to monitor stuff.
just remember when you bring up your app to use the -d if you don't want to see the tails and --build is your friend if you made changes to the code base
## Docker Lifecycle
Docker containers only run as long as necessary. So if you provide a DLL as an entry point, it will only run the container as long as it takes that DLL to do it's work and exit. You can tell if a container is still running by using the docker ps command.

## Docker Context and Docker Compose
The docker-compose context is very sensitive. It takes whatever folder you provide as context in the yaml and passes it to the docker daemon.
This context will affect the pathing in your Dockerfile, for things such as ADD and copy as it will use THAT context for its pathing.

# Useful Commands

## Docker Compose

```docker-compose up --build``` (Build the whole composition and tail)

```docker-compose up --build -d``` (Build the whole composition but don't tail)

```docker-compose run Teamcity.documentreleasenotes.app bash``` (runs just a given app that might have exited, such as the app that populates the release documents)

```docker-compose down``` (Bring down everything and destroy the containers)

```docker-compose up --build --force-recreate``` (Force the built images to not use the currently cached images)

## Docker

```docker ps``` - show all the running containers

```docker images``` - show all the images on the host

```docker rmi _image id_``` - delete an image

```docker exec -it _container id_ bash``` - this command will drop you into your container if it's still running. get the container id from a ps command

```docker container purge``` - purge all stopped containers. You'll need to do this before deleting an image

```docker ps -aq``` - List all containers (only IDs) 

```docker stop $(docker ps -aq)``` - Stop all running containers. 

```docker rm $(docker ps -aq)``` - Remove all containers. 

```docker rmi $(docker images -q)``` - Remove all images. 

From solution directory, if you want build the dockerfiles, you have to do it with a relative path
(e.g - You are at the yaml level, run the following command)

```docker build -t Teamcity.documentreleasenotes.app:latest -f Teamcity.documentreleasenotes/Dockerfile .```

# References

[Docker For Windows](https://docs.docker.com/docker-for-windows/)

[Dot Net Docker Samples](https://github.com/dotnet/dotnet-docker-samples)

[Best Practices for Docker Files](https://docs.docker.com/engine/userguide/eng-image/dockerfile_best-practices/)

[Dockerizing with CI AND CD](https://radu-matTeamcity.com/blog/aspnet-core-docker-azure/)

[Sharing Volumes Between Docker Containers](https://www.digitalocean.com/community/tutorials/how-to-share-data-between-docker-containers)

[Docker Compose](https://docs.docker.com/compose/compose-file/)

[DockerFile Docs](https://docs.docker.com/engine/reference/builder/)

[Docker Volumes](https://docs.docker.com/engine/admin/volumes/volumes/)

[Multi-Stage Builds](https://docs.docker.com/engine/userguide/eng-image/multistage-build/)

[Multi-Stage Builds with DOTNET Core](https://hub.docker.com/r/microsoft/dotnet/)

[Dot Net Core and Node JS](https://developers.filiosoft.com/docker/dotnetcore-node)

[Raneto in Docker](https://github.com/appsecco/raneto-docker)

[Multi-Container Application](https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/multi-container-microservice-net-applications/multi-container-applications-docker-compose)

[Using Service Name For Referencing Other Containers](https://docs.docker.com/docker-cloud/apps/service-links/#service-environment-variables)

[Docker Compose Networking Explained](https://docs.docker.com/compose/networking/)

[Environment Variables in Compose](https://docs.docker.com/compose/environment-variables/)

[Docker Configs](https://docs.docker.com/engine/swarm/configs/)