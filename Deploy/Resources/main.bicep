@description('Build number')
param buildNumber string

@description('Storage account name')
@minLength(3)
@maxLength(24)
param sgName string

@description('SKU name for storage account')
@allowed([
  'Standard_LRS'
  'Standard_GRS'
])
param storageSku string

@description('SKU tier for storage account')
param storageSkuTier string

@description('Function app name')
param functionAppName string

@description('Environment name')
param environmentName string

@description('Application service plan name')
param aspName string

@description('Application service plan SKU')
param planSku string

@description('Application service plan tier')
param planTier string

param sharedStorageAccount string

var appInsName = 'ins-${functionAppName}-${environmentName}'

param location string = resourceGroup().location

// Storage account
module storageAccountModule './StorageAccount/template.bicep' = {
  name: '${buildNumber}-storageaccount'
  params: {
    sgName: sgName
    sku: storageSku
    tier: storageSkuTier
    location:location
  }
}

// Application insights
module appInsightsModule 'AppInsights/template.bicep' = {
  name: '${buildNumber}-applicationinsights'
  params: {
    name: appInsName
    location:location
  }
}

// Application service plan
module aspModule 'AppServicePlan/template.bicep' = {
  name: '${buildNumber}-applicationserviceplan'
  params: {
    name: aspName
    sku: planSku
    tier: planTier
    location:location
  }
}

// Function app without settings
module functionAppModule 'FunctionApp/template.bicep' = {
  name: '${buildNumber}-functionapp'
  params: {
    name: 'fn-${functionAppName}-${environmentName}'
    planName: aspModule.outputs.planId
    location:location
  }
}

module keyVaultModule 'KeyVault/template.bicep' = {
  name: '${buildNumber}-keyvault'
  params: {
    appInsightsKey: appInsightsModule.outputs.appInsightsKey
    name: 'kv-${functionAppName}-${environmentName}'
    productionSlotPrincipalId: functionAppModule.outputs.productionPrincipalId
    stagingSlotPrincipalId: functionAppModule.outputs.stagingPrincipalId
    storageAccountConnectionString: storageAccountModule.outputs.storageAccountConnectionString
    location:location
  }
}

module functionAppSettingsModule 'FunctionAppSettings/template.bicep' = {
  name: '${buildNumber}-functionappsettings'
  params: {
    functionAppName: 'fn-${functionAppName}-${environmentName}'
    keyVaultName: 'kv-${functionAppName}-${environmentName}'
    sgName: sgName    
    sharedStorageAccount:sharedStorageAccount
  }
  dependsOn: [
    storageAccountModule
    appInsightsModule
    aspModule
    functionAppModule
    keyVaultModule
  ]
}
