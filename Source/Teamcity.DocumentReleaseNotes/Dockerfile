FROM microsoft/dotnet:2.0-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.0-sdk AS build
WORKDIR /src
COPY *.sln ./
COPY Teamcity.DocumentReleaseNotes/Teamcity.DocumentReleaseNotes.App.csproj Teamcity.DocumentReleaseNotes/
COPY Teamcity.DocumentReleaseNotes.Interfaces/Teamcity.DocumentReleaseNotes.Interfaces.csproj Teamcity.DocumentReleaseNotes.Interfaces/
COPY Teamcity.DocumentReleaseNotes.Models/Teamcity.DocumentReleaseNotes.Models.csproj Teamcity.DocumentReleaseNotes.Models/
COPY Teamcity.DocumentReleaseNotes.Business/Teamcity.DocumentReleaseNotes.Business.csproj Teamcity.DocumentReleaseNotes.Business/
RUN dotnet restore
COPY . .
WORKDIR /src/Teamcity.DocumentReleaseNotes
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

# RUN mkdir /var/logs
# RUN touch /var/logs/temp.log
# CMD tail -f /var/logs/temp.log > /dev/null

ENTRYPOINT ["dotnet", "Teamcity.DocumentReleaseNotes.App.dll"]
