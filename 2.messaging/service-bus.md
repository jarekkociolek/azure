Set subsription

```powershell
az account set --subscription <id_or_name>
```

Create Azure Service Bus Namespace

```powershell
 az servicebus namespace create -g <resource_group> -n JarekServiceBusFun
```

Create queue

```powershell
az servicebus queue create -g <resource_group> --namespace-name JarekServiceBusFun -n salesmessages
```

Create topic

```powershell
az servicebus queue create -g <resource_group> --namespace-name JarekServiceBusFun -n salesmessages
```

Get Service Bus Connection String

```powershell
az servicebus namespace authorization-rule keys list `
    --resource-group learn-8b34649b-b180-46df-a7b2-7abd42d12688 `
    --name RootManageSharedAccessKey `
    --query primaryConnectionString `
    --output tsv `
    --namespace-name JarekServiceBusFun
```

Get count messages from queue

```powershell
az servicebus queue show `
>> -g learn-8b34649b-b180-46df-a7b2-7abd42d12688 `
>> --name salesmessages `
>> --query messageCount `
>> --namespace-name JarekServiceBusFun
```
