Param(
    [string] $apiManagementInstanceName = $(throw "The API Management instance name is required"),
    [string] $resourceGroupName = $(throw "Resource group name is required"),
    [string] $policyDefinitionPath = $(throw "Path to the policy definition is required"),
    [ValidateSet('product','operation')][string]$policyType = $(throw "Policy type is required"),
    [string]$productId,
    [string]$apiId,
    [string]$operationId,
    [bool]$isLocalExecution = $false,
    [string] $subscriptionId = ""
)

function Autenticate ([string]$subscriptionId) {
    Login-AzureRmAccount
    Set-AzureRmContext -SubscriptionId $subscriptionId
}

function Verify-PolicyDefinitionLocation ([string]$policyDefinitionPath) {
    if(!(Test-Path $policyDefinitionPath)) {
        Throw [System.IO.FileNotFoundException] "A valid policy file was not found at $swaggerDefinitionPath"
    }

    Write-Host "Policy definition exists on the following location: $policyDefinitionPath"
}

function Set-AzureApiManagementPolicy ([string]$apiManagementInstanceName,[string]$resourceGroupName, [string]$policyDefinitionPath, [string]$productId, [string]$apiId, [string]$operationId) {
    
    if([string]::IsNullOrWhiteSpace($apiManagementInstanceName)){
        throw [System.Exception] "No Azure API Management instance name was specified"
    }
    if([string]::IsNullOrWhiteSpace($resourceGroupName)){
        throw [System.Exception] "No resource group name was specified"
    }

    # Create a new Azure API management context
    $apiManagementContext = New-AzureRmApiManagementContext -ResourceGroupName $resourceGroupName -ServiceName $apiManagementInstanceName
    
    # Determine what to do
    switch($policyType) {
        "product" {
            Set-AzureApiManagementPolicyForProduct -ApiManagementContext $apiManagementContext -policyDefinitionPath $policyDefinitionPath -ProductId $productId
        }
        "operation"{
            Set-AzureApiManagementPolicyForOperation  -ApiManagementContext $apiManagementContext -policyDefinitionPath $policyDefinitionPath -ApiId $apiId -OperationId $operationId
        }
    }
}

function Set-AzureApiManagementPolicyForProduct ($apiManagementContext, [string]$policyDefinitionPath, [string]$productId) {
    if($apiManagementContext -eq $null){
        throw [System.Exception] "No Azure API Management context was provided"
    }
    if(-not (Test-Path $policyDefinitionPath)){
        throw [System.IO.FileNotFoundException] "Path to the policy definition does not exist"
    }
    if([string]::IsNullOrWhiteSpace($productId)){
        throw [System.Exception] "No product Id was specified"
    }

    # Get policy content
    $policyDefinition = Get-Content $policyDefinitionPath
    Write-Host "Applying policy: $policyDefinition" -Verbose

    Set-AzureRmApiManagementPolicy -Context $apiManagementContext -PolicyFilePath $policyDefinitionPath -ProductId $productId -Verbose
    Write-Host "Policy applied to product $productId"
}

function Set-AzureApiManagementPolicyForOperation ($apiManagementContext, [string]$policyDefinitionPath, [string]$apiId, [string]$operationId) {
    if($apiManagementContext -eq $null){
        throw [System.Exception] "No Azure API Management context was provided"
    }
    if(-not (Test-Path $policyDefinitionPath)){
        throw [System.IO.FileNotFoundException] "Path to the policy definition does not exist"
    }
    if([string]::IsNullOrWhiteSpace($apiId)){
        throw [System.Exception] "No logical API Id was specified"
    }
    if([string]::IsNullOrWhiteSpace($operationId)){
        throw [System.Exception] "No operation Id was specified"
    }

    # Get policy content
    $policyDefinition = Get-Content $policyDefinitionPath
    Write-Host "Applying policy: $policyDefinition" -Verbose

    Set-AzureRmApiManagementPolicy -Context $apiManagementContext -PolicyFilePath $policyDefinitionPath -ApiId $apiId -OperationId $operationId -Verbose
    Write-Host "Policy applied to operations $operationId on api $apiId"
}

# Allow authentication when running locally
if($isLocalExecution -eq $true){
    Authenticate -subscriptionId $subscriptionId
}

# Verify that the policy definition exists
Verify-PolicyDefinitionLocation -policyDefinitionPath $policyDefinitionPath

# Apply the policy to the product
Set-AzureApiManagementPolicy -apiManagementInstanceName $apiManagementInstanceName -resourceGroupName $resourceGroupName -policyDefinitionPath $policyDefinitionPath -productId $productId -apiId $apiId -operationId $operationId