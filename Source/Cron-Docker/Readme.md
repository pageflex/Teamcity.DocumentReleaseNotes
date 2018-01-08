This image uses the [Alpine distribution](https://hub.docker.com/_/alpine/) to create a cronjob 
that connects back to the DocumentReleaseNotes API via curl, which subsequently generates all the release note 
files. crond is the actual service and it operates differently than cron in something like ubuntu.

