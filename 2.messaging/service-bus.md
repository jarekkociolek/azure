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
