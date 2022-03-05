Demo.Hotels

* :bulb: Setting RBAC using BICEP templates

https://docs.microsoft.com/en-us/azure/azure-resource-manager/bicep/scenarios-rbac

* :bulb: Built in Azure Roles

https://docs.microsoft.com/en-us/azure/role-based-access-control/built-in-roles

* :bulb: How to handle dangling role assignments when the resources are deleted from the resource group

[This can happen if you delete all the resources inside the resource group but the resources had roles assigned. These role assignments does not get removed. They need to be removed manually](https://stackoverflow.com/questions/61637124/azure-devops-pipeline-error-tenant-id-application-id-principal-id-and-scope)

* :bulb: Referencing new or existing resources using Azure Bicep

[This is needed if you would like to access the resource object itself from your bicep template](https://ochzhen.com/blog/reference-new-or-existing-resource-in-azure-bicep)

* :bulb: Azure Deployment Slots - Auto Swap

[Deploying to a staged slot and making it automatically swap](https://www.youtube.com/watch?v=RvK-VfzdzPE)

* :bulb: Learning how to deploy .NET Core apps to Azure using GitHub Actions

[Deploying an ASP.NET Core Web App to Azure](https://www.youtube.com/watch?v=cGvmbYE4HOY)


* :bulb: Creating parameter files

[Rather than passing parameters in the pipeline code, you can separate them into files](https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/parameter-files)

* :bulb: Date functions in Bicep

[Setting a custom date time format](https://docs.microsoft.com/en-us/azure/azure-resource-manager/bicep/bicep-functions-date)

* :bulb: Creating a GitHub custom action (composite)

[Custom action creation](https://docs.github.com/en/actions/creating-actions/creating-a-composite-action)

* :bulb: Using GitHub actions to deploy to Azure

https://blog.codingmilitia.com/2021/03/14/setting-up-demos-in-azure-part-2-github-actions/

* :bulb: Azure Bicep loops

[loops and conditions](https://docs.microsoft.com/en-us/azure/azure-resource-manager/bicep/loops)

* :bulb: Deploying an Azure function using Bicep

This even contains how to deploy a function app to the `production`slot.

[Mark Heath](https://markheath.net/post/azure-functions-bicep)

* :bulb: Azure Bicep scope extension resources

https://docs.microsoft.com/en-us/azure/azure-resource-manager/bicep/scope-extension-resources

* :bulb: Passing arrays and objects as secure parameters

https://ochzhen.com/blog/pass-array-and-numbers-as-secure-params

* Creating a function app slot which will contain both sensitive and non-sensitive data

If you would like to use GitHub actions, the preferred way is to create a data structure and use that to provision the slot with production and non production slots.

From GitHub actions you cannot pass "`object`" data to actions. So you will need to pass the object as a string parameter to the custom action. Doing this is somewhat tedious to be honest, but once you create the JSON data structure you need to stringify it. This [site](https://onlinetexttools.com/json-stringify-text) will help you do that much easily.

[Reference](https://github.com/Azure/bicep/discussions/6104)

* Provisioning queues inside a storage account using Bicep

In here we need to correctly set the name and the type of the child resources we are deploying. Since queue is a child resource of a storage account, either you can create them at the time you are deploying the storage account, or separately if the storage account is created earlier

[How to](https://docs.microsoft.com/en-us/azure/azure-resource-manager/bicep/child-resource-name-type)





