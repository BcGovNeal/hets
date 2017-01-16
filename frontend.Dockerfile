FROM tran-hets-tools/client
# Dockerfile for package FrontEnd
 
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

# This setting is a workaround for issues with dotnet and certain docker versions
ENV LTTNG_UST_REGISTER_TIMEOUT 0

COPY Common /app/Common
COPY FrontEnd /app/FrontEnd

WORKDIR /app/Common/src/HETSCommon
RUN dotnet restore

WORKDIR /app/FrontEnd/src/FrontEnd/
RUN dotnet restore

ENV ASPNETCORE_ENVIRONMENT Staging
ENV ASPNETCORE_URLS http://*:8080
EXPOSE 8080

RUN dotnet publish -c Release -o /app/out
WORKDIR /app
ENTRYPOINT ["dotnet", "/app/out/FrontEnd.dll"]
