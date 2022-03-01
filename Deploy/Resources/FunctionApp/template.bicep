@description('Function app name')
param name string

@description('Application service plan name')
param planName string

param location string = resourceGroup().location

resource functionAppProductionSlot 'Microsoft.Web/sites@2021-03-01' = {
  name: name
  location: location
  kind:'functionapp'
  identity:{
    type:'SystemAssigned'
  }    
  properties:{
    serverFarmId:planName            
  }  
}

resource functionAppStagingSlot 'Microsoft.Web/sites/slots@2021-03-01' = {
  name: '${functionAppProductionSlot.name}/Staging'
  location: location
  kind:'functionapp'
  identity:{
    type:'SystemAssigned'
  }  
  properties:{
    serverFarmId:planName   
    siteConfig:{
      autoSwapSlotName:'Production'
    }         
  }
  dependsOn:[
    functionAppProductionSlot
  ]
}

output productionPrincipalId string = functionAppProductionSlot.identity.principalId
output stagingPrincipalId string = functionAppStagingSlot.identity.principalId
