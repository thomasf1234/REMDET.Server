FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS installer-env

ARG VCS_REF
ARG BUILD_VERSION

ENV VCS_REF=$VCS_REF
ENV BUILD_VERSION=$BUILD_VERSION

COPY . /src/dotnet-function-app
RUN cd /src/dotnet-function-app && \
    mkdir -p /home/site/wwwroot && \
    dotnet publish *.csproj --output /home/site/wwwroot

# To enable ssh & remote debugging on app service change the base image to the one below
# FROM mcr.microsoft.com/azure-functions/dotnet:3.0-appservice
FROM mcr.microsoft.com/azure-functions/dotnet:3.0

ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

# Set standard labels 
LABEL "com.example.name"="REMDET.Server"
LABEL "com.example.description"="A function app responsible for accepting and saving bodily sensor events"
LABEL "com.example.vcs-url"="https://github.com/thomasf1234/REMDET.Server"
LABEL "com.example.vcs-ref"=$VCS_REF
LABEL "com.example.vendor"="Example Company"
LABEL "com.example.schema-version"="1.0"
LABEL "com.example.build-version"=$BUILD_VERSION

# Set non-standard Labels
LABEL "com.example.component"="function"
LABEL "com.example.part-of"="ExampleFitnessProject"

COPY --from=installer-env ["/home/site/wwwroot", "/home/site/wwwroot"]