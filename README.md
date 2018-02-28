# Teamcity.DocumentReleaseNotes

## Summary

Pageflex uses Enterprise-style components with SaaS installers to deliver business functionality to the Storefront Pando platform. Teamcity is a major component of this process as it creates builds of the different components that are pulled from different feature branches. There was a need to be able to gather release documentation as these components accrued versions through the build cycle. EI.DocumentReleaseNotes is a suite of apps and docker containers that fulfills this request in an automated fashion and eventually exposes them through a website.

## Solution Composition
The full solution is composed of 4 separate Docker images, each housing their own application. 
Each play a part in how the solution works to deliver the business functionality.

1. Raneto Docker Image
2. Release Notes App and Image
3. Release Notes API and Image
4. Chron Job Docker Image

## Docker Summary

Docker is a container / image technology that allows for easy segregation and eventual deployment of code and system. A container is a running image. An image is an u


### Raneto

[Raneto](http://raneto.com/) is a free solution that delivers a website based on file folders and markdown files. The Docker image we are using was [freely distributed with a Dockerfile](https://github.com/appsecco/raneto-docker) and allows us to customize the eventual site that stores our release notes.

### Release Notes App

Based on dot net core 2, this app shares the business layer with the API, and runs from a bash script or command line and generates the necessary files that hold all the change notes for each build type. When running the full container, the app executes, writes the release note files, and then terminates its container. The projects it queries is based on configuration.

### Release Notes API

There is a lot of data that can be mined from Teamcity, but it requires pretty distinct knowledge of the Teamcity API. We've encapsulated a lot of that information into our own API to make it simpler to gather the knowledge in one fell swoop. The API is Dot Net Core 2 built and subsequently hosted in a linux docker image. In the API itself, There are methods to just gather data, and also a method that generates the documents needed by Raneto to power the release notes website.

**Settings API**

* api/Settings/GetConfiguredProjects [GET] - Get all projects ids that are configured for building their change notes

**ReleaseNotes API**

* api/ReleaseNotes/Create [GET] - Start the whole process of creating the change notes.
* api/ReleaseNotes/GetConfigured [GET] - Check which projects are configured for release note creation.
* api/ReleaseNotes/Project/{ProjectName} [GET] - Gets all the Release notes in json format for a given projects build types.
* api/ReleaseNotes/BuildType/{BuildType} [GET] - Gets Release notes in json format for a given build type

### Chron Job Container/Image

To continue to maintain the website, a chron job Image will be created that calls the API on a scheduled basis to allow it to re-generate the release notes on a schedule. This way, our website stays fresh with the latest builds and their changes.

## How and Where to Configure

Both the app and the api have separate configuration files that sit in the root directory called appsettings.json. In this file, the TC credentials and Projects we are interested in reside.

### App

if the projects are empty, it will try and pull the configured projects from the api

teamcityapi.json is not to be touched and is where the api end points for teamcity are configured. These should only be changed if Teamcity is versioned and they change their API. The api is currently based on Teamcity 10.

## How to use the whole Solution in VS.Net for Development

There are 2 ways to use the solution from vs.net  First way is letting VS.NET do all the composition and running and building. This can be done by right clicking on the docker-compose project and "debug". It will load everything for you. This is the preferred method if you want to debug the differen applications The second way is to use the "terminal window" in "other windows" to navigate to the solution folder and use ```docker-compose up --build -d``` to start the solution. The --build is specifically to re-build anything in case you made any changes and the -d is to not stay in the tailing of the logs.  ```docker-compose down``` properly terminates all the containers in a very clean manner.

## Shared Volumes

The App, the API, and Raneto all use a shared volume to distribute the release notes, so it doesn't matter if the App or the API containers create the notes, Raneto receives them from both and immediately displays any new files and/or changes.

## Accessing the individual containers

If you need to access the containers, they need to be up. This might sound easy, and it is for the Raneto container and the API container as these stay up to serve information and files. If the container is running, any Powershell can be used to run the following commands

```docker ps``` - copy the container id to be used in the next command

```docker exec -it _container id here_ bash``` - If the container is running, it will drop you into the workdir of the container in question.

If the container is not running (such as is usually the case for the app), then it's a little bit harder. Introduce a hard break in the code so that it pauses execution, and then the above commands can be used to access the app's Docker container.

To spin up the app container (since it's lifecycle is only as long as it takes to write the documents) and re-write the doc files to the Raneto website, run the following commands in a Powershell that it at the solution folder level,

```docker-compose up --build -d``` will spin all the containers as long as it is run from a command line that is at the solution location. Wait for the app to fall off the docker ps. Then run the following command:

```docker-compose run teamcity.documentreleasenotes.app bash```

This command will spin up the app again and write the files again.

To run just the app alone, the following command can be used at a command line that is at the solution level path.

```docker build -t teamcity.documentreleasenotes.app:latest -f teamcity.documentreleasenotes/Dockerfile .```

This will run and terminate the app container without having to run the full docker-compose
