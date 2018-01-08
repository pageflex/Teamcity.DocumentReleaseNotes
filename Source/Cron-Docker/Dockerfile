FROM alpine:3.5

ENV ApiUrl="http://Teamcity.documentreleasenotes.api/api/ReleaseNotes/Create"

# Add curl for later use
RUN apk --no-cache add curl

# RUN rc-service crond start && rc-update add crond

# add a busy-box crond task
# https://gist.github.com/andyshinn/3ae01fa13cb64c9d36e7

# COPY update-document-with-curl /etc/periodic/15min/call-curl-api
RUN touch /etc/periodic/daily/call-curl-create-documents
RUN { echo "#!/bin/sh"; echo "/usr/bin/curl -s -o /dev/null -w "'"%{http_code}"'" $ApiUrl";} > /etc/periodic/daily/call-curl-create-documents
RUN chmod a+x /etc/periodic/daily/call-curl-create-documents

RUN touch /var/log/cron.log

CMD crond -l 2 -f -L /var/log/cron.log

# FROM ubuntu:latest
# MAINTAINER docker@ekito.fr

# RUN apk --no-cache add curl
 
# Add crontab file in the cron directory
# ADD crontab /etc/cron.d/hello-cron
 
# Give execution rights on the cron job
# RUN chmod 0644 /etc/cron.d/hello-cron
 
# Create the log file to be able to run tail
# RUN touch /var/log/cron.log
 
# Run the command on container startup
# CMD cron && tail -f /var/log/cron.log