param sharedResourceGroup string
param sharedStorageAccount string

@secure()
param productionPrincipalId string

@secure()
param stagingPrincipalId string

@description('This is the built-in storage queue data contributor role. See https://docs.microsoft.com/azure/role-based-access-control/built-in-roles#contributor')
resource storageQueueDataContributor 'Microsoft.Authorization/roleDefinitions@2018-01-01-preview' existing = {
  scope: subscription()
  name: '974c5e8b-45b9-4653-ba55-5f855dd0fb88'
}

resource sharedStg 'Microsoft.Storage/storageAccounts@2021-02-01' existing = {
  scope: resourceGroup()  
  name: sharedStorageAccount
}

resource storageQueueDataContributorProductionAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  name: guid(resourceGroup().id, 'productionSlot', storageQueueDataContributor.id)  
  scope:sharedStg
  properties: {
    roleDefinitionId: storageQueueDataContributor.id
    principalId: productionPrincipalId
    principalType: 'ServicePrincipal'
  }  
}

resource storageQueueDataContributorStagingAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  name: guid(resourceGroup().id, 'stagingSlot', storageQueueDataContributor.id)
  scope:sharedStg
  properties: {
    roleDefinitionId: storageQueueDataContributor.id
    principalId: stagingPrincipalId
    principalType: 'ServicePrincipal'
  }  
}

