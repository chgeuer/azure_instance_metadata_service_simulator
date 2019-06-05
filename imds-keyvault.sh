#!/usr/bin/env bash

apiVersion="2018-02-01"

curl --request GET \
    --silent \
    -H Metadata:true \
    "http://169.254.169.254/metadata/identity/oauth2/token?api-version=${apiVersion}&client_id=${service_principal_application_id:=deadbeef-efa9-42ea-9ac3-28a920370be6}&resource=https%3A%2F%2Fvault.azure.net"

# curl --request GET --silent -H Metadata:true  "http://169.254.169.254/metadata/identity/oauth2/token?api-version=2018-02-01&client_id=deadbeef-efa9-42ea-9ac3-28a920370be6&resource=https%3A%2F%2Fvault.azure.net"
