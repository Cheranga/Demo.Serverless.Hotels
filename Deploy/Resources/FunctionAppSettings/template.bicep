@description('Function app name')
param functionAppName string

@description('Key vault name')
param keyVaultName string

@description('Storage account name')
param sgName string

param sharedStorageAccount string

var storageAccountConnectionStringSecret = '@Microsoft.KeyVault(SecretUri=https://${keyVaultName}.vault.azure.net/secrets/storageAccountConnectionString/)'
var appInsightsKeySecret = '@Microsoft.KeyVault(SecretUri=https://${keyVaultName}.vault.azure.net/secrets/appInsightsKey/)'
var timeZone = 'AUS Eastern Standard Time'
var queue = 'https://${sharedStorageAccount}.queue.core.windows.net'

resource productionSlotAppSettings 'Microsoft.Web/sites/config@2021-02-01' = {
  name: '${functionAppName}/appsettings'
  properties:{
    CustomerApiKey: 'This is the production setting'      
    AzureWebJobsStorage__accountName: sgName
    WEBSITE_CONTENTAZUREFILECONNECTIONSTRING: storageAccountConnectionStringSecret
    WEBSITE_CONTENTSHARE: toLower(functionAppName)
    FUNCTIONS_EXTENSION_VERSION: '~4'
    APPINSIGHTS_INSTRUMENTATIONKEY: appInsightsKeySecret
    FUNCTIONS_WORKER_RUNTIME: 'dotnet'
    WEBSITE_TIME_ZONE: timeZone
    WEBSITE_ADD_SITENAME_BINDINGS_IN_APPHOST_CONFIG: 1  
  }
}

resource stagingSlotAppSettings 'Microsoft.Web/sites/slots/config@2021-02-01'= {
  name: '${functionAppName}/Staging/appsettings'
  properties:{
    CustomerApiKey: 'This is the staging setting'  
    AzureWebJobsStorage__accountName: sgName
    HotelCancellationQueue: 'hotel-cancellations'
    QueueSource__queueServiceUri: queue
    WEBSITE_CONTENTAZUREFILECONNECTIONSTRING: storageAccountConnectionStringSecret
    WEBSITE_CONTENTSHARE: toLower(functionAppName)
    FUNCTIONS_EXTENSION_VERSION: '~3'
    APPINSIGHTS_INSTRUMENTATIONKEY: appInsightsKeySecret
    FUNCTIONS_WORKER_RUNTIME: 'dotnet'
    WEBSITE_TIME_ZONE: timeZone
    WEBSITE_ADD_SITENAME_BINDINGS_IN_APPHOST_CONFIG: 1
  }
}
