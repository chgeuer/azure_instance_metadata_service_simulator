# A small utility to simulate the Azure Instance Metadata Service on a dev box

The 'Azure Instance Metadata Service' (IMDS) is a REST endpoint, available at a well-known non-routable IP address (`169.254.169.254`), on a variety of Azure compute resources, such as [Virtual Machines](https://docs.microsoft.com/en-us/azure/virtual-machines/linux/instance-metadata-service) or also on Azure Container Instances. You can find various [samples](https://github.com/microsoft/azureimds) or [articles](http://blog.geuer-pollmann.de/blog/2019/02/28/call-azure-arm-api-with-curl/) how to call that service.

The problem however is that developing 'locally' can be cumbersome, because the service is not available. The simple Go utility here essentially mimics IMDS.

## The 'magic' IP `169.254.169.254`

For that to work, you need to bind that special IP address to one of your network interfaces. For example, on Windows, I run an administrative command prompt, and run this:

```batch
netsh.exe interface ipv4 add address ^
    name="vEthernet (Default Switch)" ^
    address="169.254.169.254"
```

To remove that binding, you can run this:

```batch
netsh.exe interface ipv4 delete address ^
    name="vEthernet (Default Switch)" ^
    address="169.254.169.254"
```

## Configure the service

The application looks at a small config file, and exposes the file's information via REST:

```json
{
    "tenantID": "sometenant.onmicrosoft.com",
    "servicePrincipals": {
        "deadbeef-efa9-42ea-9ac3-28a920370be6": "ewewM/OblEL/WOJweC0="
    },
    "metadata": {
        "compute": {
            "provider":"Microsoft.Compute",
            "tags":"tag1:val2",
            "azEnvironment":"AzurePublicCloud",
            "location":"westeurope",
            "subscriptionId":"deadbeef-bee4-484b-bf13-d6a5505d2b51",
            "resourceGroupName":"spring",
            "vmId":"c7619932-27e3-4a63-988c-460bd290ca55",
            "name":"somevm",
            "vmSize":"Standard_D2s_v3",
            "customData":"",
            "placementGroupId":"", "platformFaultDomain":"0", "platformUpdateDomain":"0",
            "osType":"Linux",
            "publisher":"Canonical", "offer":"UbuntuServer", "sku":"18.04-LTS", "version":"18.04.201905290",
            "vmScaleSetName":"",
            "zone":""
        }
    }
}
```

Essentially, the `.metadata` part is exposed on 'http://169.254.169.254/metadata/instance/compute'... 

## Managed user-assigned Identity

One of the funky capabilities of IMDS is that it can fetch Azure AD tokens on behalf of the application, without exposing credentials such as service principal keys to the application. For example, this script expects to fetch a token for Azure KeyVault:

```bash
#!/bin/bash

apiVersion="2018-02-01"

curl --request GET \
    --silent \
    -H Metadata:true \
    "http://169.254.169.254/metadata/identity/oauth2/token?api-version=${apiVersion}&client_id=${service_principal_application_id}&resource=https%3A%2F%2Fvault.azure.net"
```

Managed Identities can, by design, not be directly reachable on your developer laptop (unless we would proxy that REST call to a real Azure compute resource). To mimic that token endoint (`/metadata/identity/oauth2/token`), you can configure service principal credentials in the config file as well.
