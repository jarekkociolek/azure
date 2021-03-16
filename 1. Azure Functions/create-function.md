Create storage where Azure Function will keep its configuration

```powershell
$storageAccountName = "azurefunctionsstoragetest"
$location = "westeurope"
$resourceGroup = "myResourceGroup"

az storage account create
-n $storageAccountName
-l $location
-g $resourceGroup
--sku Standard_LRS
```

Create Function App which is a logical container for functions

```powershell

$functionAppName = "azurefunctionstest"

az functionapp create
  -n $functionAppName
  --storage-account $storageAccountName
  --consumption-plan-location $location
  --runtime dotnet `
  -g $resourceGroup
```

Create a function project locally

```powershell
func init DemoFunction --dotnet
```

Create a function inside project locally

```powershell
func new --name DemoFunction --template "HTTP trigger" --authlevel "anonumous"
```

Timer function created with command:
```powershell
func new --name "TimeTriggerFunction" --template "Timer trigger" --language "csharp"
```

Http function created with command:
```powershell
func new --name 'HttpTriggerFunction' --template 'HTTP trigger' --language 'csharp'
```
