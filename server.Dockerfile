FROM microsoft/dotnet:1.1.0-sdk-projectjson
# Dockerfile for package HETSAPI

ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

# This setting is a workaround for issues with dotnet and certain docker versions
ENV LTTNG_UST_REGISTER_TIMEOUT 0

COPY Common /app/Common

WORKDIR /app/Common/src/HETSCommon
RUN dotnet restore

COPY Server /app/Server
WORKDIR /app/Server/src/HETSAPI/
RUN dotnet restore

ENV ASPNETCORE_ENVIRONMENT Staging
ENV ASPNETCORE_URLS http://*:8080
EXPOSE 8080

RUN dotnet publish -c Release -o /app/out
WORKDIR /app/out
ENTRYPOINT ["dotnet", "/app/out/HETSAPI.dll"]
