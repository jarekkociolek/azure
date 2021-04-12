Create signalr service with random name

```powershell
SIGNALR_SERVICE_NAME=msl-sigr-signalr$(openssl rand -hex 5)
RESOURCE_GROUP_NAME=<fill resource group>
az signalr create \
  --name $SIGNALR_SERVICE_NAME \
  --resource-group $RESOURCE_GROUP_NAME \
  --sku Free_DS2 \
  --unit-count 1
```
