FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY Teamcity.DocumentReleaseNotes.Business/Teamcity.DocumentReleaseNotes.Business.csproj Teamcity.DocumentReleaseNotes.Business/
COPY Teamcity.DocumentReleaseNotes.Interfaces/Teamcity.DocumentReleaseNotes.Interfaces.csproj  Teamcity.DocumentReleaseNotes.Interfaces/
COPY Teamcity.DocumentReleaseNotes.Models/Teamcity.DocumentReleaseNotes.Models.csproj Teamcity.DocumentReleaseNotes.Models/
COPY Teamcity.DocumentReleaseNotes.Api/Teamcity.DocumentReleaseNotes.Api.csproj Teamcity.DocumentReleaseNotes.Api/
RUN dotnet restore
COPY . .
WORKDIR /src/Teamcity.DocumentReleaseNotes.Api
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
RUN mkdir /app/Documents
VOLUME /app/Documents

RUN mkdir /app/Logs
VOLUME /app/Logs

ENTRYPOINT ["dotnet", "Teamcity.DocumentReleaseNotes.Api.dll"]
